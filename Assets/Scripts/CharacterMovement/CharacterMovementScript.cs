using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement du Personnage")]
    public float jumpForce = 10f; // Force du saut
    public float slideDuration = 0.5f; // Durée de la glissade
    public float breakableCheckRadius = 0.2f; // Rayon pour vérifier le sol
    public float objectDetectionDistance = 1f; // Distance horizontale de détection des objets

    [Header("References")]
    public Transform groundCheck; // Point pour vérifier si le personnage est au sol
    public LayerMask groundLayer; // Couche utilisée pour détecter le sol
    public BoxCollider2D breakableDetector; // Point pour détecter les objets devant
    public BoxCollider2D normalCollider; // Collider standard du personnage
    

    private Rigidbody2D rb;
    private Animator animator;
    private bool canSlide = false;
    private bool isSliding = false;
    private bool isGrounded = false; // Indique si le personnage est au sol
    private bool BreakableObjectDetected = false; // Détection du bloc cassable
    private float[] previousY; // Position Y précédente du personnage

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previousY = new float[2];
        previousY[0] = transform.position.y;
        previousY[1] = float.MaxValue;
    }

    void Update()
    {
        // Met à jour l'état d'être au sol
        UpdateGrounded();

        // Détection d'un objet cassable devant le joueur
        CheckForBreakableObject();
        Debug.Log(BreakableObjectDetected);
        // Saut
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSliding)
        {
            Jump();
        }

        // Glissade
        if (Input.GetKeyDown(KeyCode.DownArrow) && canSlide && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    // Méthode de saut
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Utilisation de rb.velocity au lieu de linearVelocity
        animator.SetTrigger("Jump");
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
        Debug.Log("EJ SLIDE");
        animator.SetTrigger("Slide");

        // Réduit temporairement la taille du collider pour simuler la glissade
        Vector2 originalSize = normalCollider.size;
        Vector2 originalOffset = normalCollider.offset;

        normalCollider.size = new Vector2(originalSize.x, originalSize.y / 2);
        normalCollider.offset = new Vector2(originalOffset.x, originalOffset.y / 2);

        yield return new WaitForSeconds(slideDuration);

        // Restaure le collider
        normalCollider.size = originalSize;
        normalCollider.offset = originalOffset;
        Debug.Log("FIN SLIDE");
        isSliding = false;
    }

    void UpdateGrounded()
    {
        // Vérifie si le personnage est en contact avec le sol
        if (previousY[1] != float.MaxValue) isGrounded = Math.Abs(previousY[0] - transform.position.y) + Math.Abs(previousY[1] - transform.position.y) < 0.002f;
        previousY[1] = previousY[0];
        previousY[0] = transform.position.y;
    }

    // Détection des objets cassables via le trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        Breakable breakableObject = collision.GetComponent<Breakable>();
        if (breakableObject != null)
        {
            if (isSliding)
            {
                // Si le personnage est en glissade, on détruit l'objet cassable
                Destroy(breakableObject.gameObject);  // Détruit l'objet
                Debug.Log("Objet cassable détruit");
            }
            else
            {
                // Si le personnage n'est pas en glissade, la glissade est autorisée
                canSlide = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Vérifie si l'objet sortant possède un composant Breakable
        if (collision.GetComponent<Breakable>() != null)
        {
            canSlide = false; // L'objet cassable a quitté la zone, la glissade n'est plus autorisée
        }
    }

    // Visualisation dans l'éditeur du rayon de détection du sol
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, breakableCheckRadius);
        }
    }

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
}
