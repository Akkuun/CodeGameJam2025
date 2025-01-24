using UnityEngine;

public class DoubleJumpObsttacle : MonoBehaviour
{
    public float jumpPadForce = 15f; // La force appliquée par le JumpPad

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            // Applique la force de saut au joueur
            player.ActivateJumpPad(jumpPadForce);
        }
    }
}
