

using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int score;
    public int levelPosition = 0;
    public AudioSource explosionSource;

    [Header("Mouvement du Personnage")]
    public float jumpForce = 30f; // Force du saut
    public float secondJumpForce = 30f; // Force du deuxième saut (si nécessaire, peut être différente)
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
    private ScrollManager gameManager;

    private Rigidbody2D rb;
    private Animator animator;
    private bool canSlide = false;
    private bool isGrounded = false; // Indique si le personnage est au sol
    private bool BreakableObjectDetected = false; // Détection du bloc cassable
    private float[] previousY; // Position Y précédente du personnage
    private bool isJumping = false; // Indique si le joueur est en train de sauter
    private bool canSingleJump = false; // Indique si un saut simple est possible


    private bool canDoubleJump = false; // Indique si le joueur peut effectuer un double saut

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previousY = new float[2];
        previousY[0] = transform.position.y;
        previousY[1] = float.MaxValue;
        gameManager = ScrollManager.instance;
        levelPosition = 0;
    }

    
    void Update()
    {

        Debug.Log(" Hello " + secondJumpForce);
        if (gameManager.gameState == GameState.Title || gameManager.gameState == GameState.GameOver)
        {
            return;
        }
        // Met à jour l'état d'être au sol
        UpdateGrounded();

        CheckIfPlayerIsDead();

        // Saut si flèche du haut + AU SOL + NE GLISSE PAS

        //Double saut
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded && canDoubleJump && !isSliding)
        {
            Debug.Log(secondJumpForce); 
            DoubleJump(secondJumpForce);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSliding)
        {
            Jump(jumpForce);
            //canSingleJump = false; // Désactive le saut simple jusqu'à ce que le joueur touche le sol
        }
        //// Glissade
        if (Input.GetKeyDown(KeyCode.DownArrow) && canSlide && !isSliding)
        {
            StartCoroutine(Slide());
        }

        // Score 
        if (ScrollManager.instance != null)
        {
            score = Mathf.FloorToInt(ScrollManager.instance.distanceScrolled);
        }

        //Debug.Log(isGrounded);

        
    }

    // Méthode de saut
    void Jump(float force)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, force); // Utilisation de rb.velocity pour appliquer la force du saut
            animator.SetTrigger("Jump");
        }

        
    }

    // Double saut
    void DoubleJump(float force)
    {

        Debug.Log(force);
        if (canDoubleJump)
        {

            //rb.AddForce(new Vector2(0, force - rb.linearVelocity.y), ForceMode2D.Impulse);
            //Debug.Log(force);
            //animator.SetTrigger("DoubleJump");
            //canDoubleJump = false; // Désactive le double saut après utilisation
            //rb.linearVelocity = new Vector2(0, 0);
            rb.linearVelocity = new Vector2(0, force);

            animator.SetTrigger("DoubleJump");
            canDoubleJump = false;
        }
        
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
        RaycastHit2D[] raytab = new RaycastHit2D[1];
        int t = normalCollider.Raycast(new Vector2(0, -1), raytab, 1.2f/2f);
        isGrounded = t > 0 && !(raytab[0].collider.tag == "DoubleJumpObject");



        // Vérifie si le personnage est en contact avec le sol
        //if (previousY[1] != float.MaxValue)
        //{
        //    isGrounded = Math.Abs(previousY[0] - transform.position.y) + Math.Abs(previousY[1] - transform.position.y) < 0.0000001d;
        //}
        //previousY[1] = previousY[0];
        //previousY[0] = transform.position.y;

        // Si le personnage est au sol, on réactive le double saut
        //if (isGrounded)
        //{
        //    // Réinitialise les sauts lorsqu'au sol
        //    canSingleJump = true;
        //    canDoubleJump = false;
        //    isJumping = false;
        //}
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
        if (collision.tag == "DoubleJumpObject")
        {
            canDoubleJump = true;
        }
        //detection avec un Jumping pad
        if (normalCollider.IsTouching(collision) && collision.GetComponent<JumpingPadObject>() != null)
        {
           
            ActivateJumpPad(25f); // Appliquer l'effet du JumpPad
        }

        if (normalCollider.IsTouching(collision) && collision.GetComponent<Ground>() != null)
        {

            isGrounded = true;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Breakable>() != null)
        {
            canSlide = false;
        }

        // Réinitialise la possibilité de double saut lorsque le joueur quitte l'objet
        if (collision.tag == "DoubleJumpObject")
        {
            canDoubleJump = false;
        }

        if (collision.GetComponent<DoubleJumpObsttacle>() != null)
        {
            canDoubleJump = false; // Désactive le double saut lorsque le joueur quitte l'objet
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
        //canDoubleJump = true; // Permet un double saut après un saut normal
        Debug.Log("heh");
    
    
        }
}
