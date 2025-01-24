using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement du Personnage")]
    public float jumpForce = 10f;      // Force du saut
    public float slideDuration = 0.5f; // Dur�e de la glissade
    public float groundCheckRadius = 0.2f; // Rayon pour v�rifier le sol
    public LayerMask groundLayer;     // Couche pour d�tecter le sol

    [Header("References")]
    public Transform groundCheck; // Point sous le personnage pour verifier le sol
    public BoxCollider2D normalCollider; // Collider standard du personnage

    private Rigidbody2D rb;
    private Animator animator;
    private bool isSliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Saut
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isSliding)
        {
            Jump();
        }

        // Glissade
        if (Input.GetKeyDown(KeyCode.S) && IsGrounded() && !isSliding)
        {
            StartCoroutine(Slide());

            // Change temporairement le scale en Y du personnage pour le réduire
            Vector3 originalScale = transform.localScale;
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

            // Restaure le scale après la durée de la glissade
            StartCoroutine(ResetScale(originalScale, slideDuration));
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
        animator.SetTrigger("Slide");

        // R�duire temporairement la taille du collider pour simuler la glissade
        Vector2 originalSize = normalCollider.size;
        Vector2 originalOffset = normalCollider.offset;

        normalCollider.size = new Vector2(originalSize.x, originalSize.y / 2);
        normalCollider.offset = new Vector2(originalOffset.x, originalOffset.y / 2);

        yield return new WaitForSeconds(slideDuration);

        // Restaurer le collider
        normalCollider.size = originalSize;
        normalCollider.offset = originalOffset;

        isSliding = false;
    }

    bool IsGrounded()
    {
        // V�rifie si le personnage touche le sol
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return hit != null;
    }

    void OnDrawGizmosSelected()
    {
        // Affiche un cercle dans l'�diteur pour visualiser le point de v�rification du sol
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
