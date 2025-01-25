using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement du Personnage")]
    public float jumpForce = 10f; // Force du saut
    public float secondJumpForce = 8f; // Force du deuxième saut (si nécessaire, peut être différente)
    public float slideDuration = 1f; // Durée de la glissade
    public float breakableCheckRadius = 0.2f; // Rayon pour vérifier le sol
    public float objectDetectionDistance = 1f; // Distance horizontale de détection des objets

    [Header("Références")]
    public Transform groundCheck; // Point pour vérifier si le personnage est au sol
    public LayerMask groundLayer; // Couche utilisée pour détecter le sol
    public LayerMask breakableLayer; // Couche des objets cassables
    public BoxCollider2D breakableDetector; // Point pour détecter les objets devant
    public BoxCollider2D normalCollider; // Collider standard du personnage
    public bool isDead = false; // variable connue de tous pour savoir quand le joueur meurt
    public bool isSliding = false; // variable connue de tous pour savoir quand le joueur glisse
    public AudioSource deathSFX; // Son de mort du joueur
    public AudioSource jumpSFX; // Son de saut du joueur
    public AudioSource doubleJumpSFX; // Son de double saut du joueur
    public AudioSource jumpPadSFX; // Son du JumpPad
    private ScrollManager gameManager;
    private MusicManager musicManager;

    private Rigidbody2D rb;
    private Animator animator;
    private bool canSlide = false;
    private bool isGrounded = false; // Indique si le personnage est au sol
    private bool BreakableObjectDetected = false; // Détection du bloc cassable
    private float[] previousY; // Position Y précédente du personnage

    private bool canDoubleJump = false; // Indique si le joueur peut effectuer un double saut

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previousY = new float[2];
        previousY[0] = transform.position.y;
        previousY[1] = float.MaxValue;
        gameManager = ScrollManager.instance;
        musicManager = MusicManager.instance;
    }

    void Update()
    {
        if (gameManager.gameState == GameState.Title || gameManager.gameState == GameState.GameOver)
        {
            return;
        }
        // Met à jour l'état d'être au sol
        UpdateGrounded();

        CheckIfPlayerIsDead();

        // Saut
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSliding)
        {
            Jump(jumpForce); // Saut normal
        }
        // Double saut
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded && canDoubleJump && !isSliding)
        {
            DoubleJump(jumpForce); // Double saut
        }

        // Glissade
        if (Input.GetKeyDown(KeyCode.DownArrow) && canSlide && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    // Méthode de saut
    void Jump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force); // Utilisation de rb.velocity pour appliquer la force du saut
        animator.SetTrigger("Jump");
        canDoubleJump = true; // Permet un double saut après un saut normal
        jumpSFX.Play();
    }

    // Double saut
    void DoubleJump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force); // Applique la force du double saut
        animator.SetTrigger("DoubleJump");
        canDoubleJump = false; // Désactive le double saut après utilisation
        doubleJumpSFX.Play();
    }

    private IEnumerator ResetScale(Vector3 originalScale, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restaure le scale d'origine
        transform.localScale = originalScale;
    }

    IEnumerator Slide()
    {
        isSliding = true;

        // Détection d'un objet cassable devant le joueur
        animator.SetTrigger("Slide");

        // Réduit temporairement la taille du collider pour simuler la glissade
        Vector2 originalSize = normalCollider.size;
        normalCollider.size = new Vector2(originalSize.x, originalSize.y / 2);

        yield return new WaitForSeconds(slideDuration);

        // Restaure le collider
        normalCollider.size = originalSize;

        isSliding = false;
    }

    void UpdateGrounded()
    {
        // Vérifie si le personnage est en contact avec le sol
        if (previousY[1] != float.MaxValue) isGrounded = Math.Abs(previousY[0] - transform.position.y) + Math.Abs(previousY[1] - transform.position.y) < 0.002f;
        previousY[1] = previousY[0];
        previousY[0] = transform.position.y;

        // Si le personnage est au sol, on réactive le double saut
        if (isGrounded)
        {
            canDoubleJump = false;
        }
    }

    // Détection des objets cassables via le trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        Breakable breakableObject = collision.GetComponent<Breakable>();
        if (breakableObject != null)
        {
            if (isSliding)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                canSlide = true;
            }
        }

        // Détection de l'objet de double saut
        if (collision.GetComponent<DoubleJumpObsttacle>() != null)
        {
            canDoubleJump = true;
        }
        //detection avec un Jumping pad
        if (normalCollider.IsTouching(collision) && collision.GetComponent<JumpingPadObject>() != null)
        {
           
            ActivateJumpPad(25f); // Appliquer l'effet du JumpPad
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Breakable>() != null)
        {
            canSlide = false;
        }

        // Réinitialise la possibilité de double saut lorsque le joueur quitte l'objet
       if (normalCollider.IsTouching(collision) && collision.GetComponent<JumpingPadObject>() != null)
        {
            canDoubleJump = false;
        }
    }

    // Fonction qui met BreakableObjectDetected si un Breakable entre dans la box collider
    void CheckForBreakableObject()
    {
        // Vérifie la détection d'un objet cassable devant le joueur en utilisant OverlapBox
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(breakableDetector.bounds.center, breakableDetector.bounds.size, 0f);
        BreakableObjectDetected = false; // Réinitialise la détection

        foreach (var hitCollider in hitColliders)
        { 
            if (hitCollider != null && hitCollider.GetComponent<Breakable>() != null)
            {
                // Si un objet de type Breakable est trouvé, on met à jour la variable
                BreakableObjectDetected = true;

                break; // On peut sortir de la boucle dès qu'on détecte un bloc cassable
            }
        }
    }

    void CheckIfPlayerIsDead()
    {
        if (gameObject.transform.position.x < -0.1)
        {
            triggerDeath();
        }
        if (isDead)
        {
           // Debug.Log("Le joueur est MORT");
        }
    }

    public void ActivateJumpPad(float jumpPadForce)
    {
        // Applique une force verticale spécifique pour le JumpPad
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPadForce); // Utilisation de rb.velocity pour appliquer la force du saut
        animator.SetTrigger("Jump");
        canDoubleJump = true; // Permet un double saut après un saut normal
    
        jumpPadSFX.Play();
    }

    public void triggerDeath() {
        isDead = true;
        gameManager.gameState = GameState.GameOver;
        musicManager.StopAllCoroutines();
        musicManager.stopAll();
        deathSFX.Play();
    }
}
