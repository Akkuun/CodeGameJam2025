using UnityEngine;

public class DoubleJumpObsttacle : MonoBehaviour
{
    public float jumpPadForce = 15f; // La force appliquée par le JumpPad

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision détectée avec : {collision.name}");



        // Vérifie si l'objet entrant est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("CHARACTERR ENCONTRE");
            // Applique la force de saut au joueur
            player.ActivateJumpPad(jumpPadForce);
        }
    }
}
