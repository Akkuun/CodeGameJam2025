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
    public Transform breakableDetector; // Point pour détecter les objets devant
    public BoxCollider2D normalCollider; // Collider standard du personnage

    private Rigidbody2D rb;
    private Animator animator;
    private bool isSliding = false;
    private bool isGrounded = false; // Indique si le personnage est au sol
    // flaot array
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
        //Debug.Log($"Gournded{isGrounded}");

        // Saut
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSliding)
        {
            print("Jump");
            print("isGrounded" + isGrounded);
            Jump();
        }

        // Glissade
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && !isSliding)
        {
            if (IsObjectInFront("Breakable"))
            {
                StartCoroutine(Slide());

                // Change temporairement le scale en Y du personnage pour le réduire
                Vector3 originalScale = transform.localScale;
                transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

                // Restaure le scale après la durée de la glissade
                StartCoroutine(ResetScale(originalScale, slideDuration));
            }
        }
    }

    // Méthode de saut
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

        // Réduit temporairement la taille du collider pour simuler la glissade
        Vector2 originalSize = normalCollider.size;
        Vector2 originalOffset = normalCollider.offset;

        normalCollider.size = new Vector2(originalSize.x, originalSize.y / 2);
        normalCollider.offset = new Vector2(originalOffset.x, originalOffset.y / 2);

        yield return new WaitForSeconds(slideDuration);

        // Restaure le collider
        normalCollider.size = originalSize;
        normalCollider.offset = originalOffset;
        isSliding = false;
    }

    void UpdateGrounded()
    {
        // Vérifie si le personnage est en contact avec le sol
        if (previousY[1] != float.MaxValue) isGrounded = Math.Abs(previousY[0] - transform.position.y) + Math.Abs(previousY[1] - transform.position.y) < 0.002f;
        previousY[1] = previousY[0];
        previousY[0] = transform.position.y;
    }


    bool IsObjectInFront(string tag)
    {
        // Raycast vers l'avant pour détecter un objet avec le tag spécifié
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, objectDetectionDistance);
        if (hit.collider != null && hit.collider.CompareTag(tag))
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Affiche un cercle dans l'éditeur pour visualiser le point de vérification du sol
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, breakableCheckRadius);
        }
    }
}
