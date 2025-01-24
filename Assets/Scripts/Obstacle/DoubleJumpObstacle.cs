using UnityEngine;

public class DoubleJumpObsttacle : MonoBehaviour
{
    public float jumpPadForce = 15f; // La force appliqu�e par le JumpPad

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision d�tect�e avec : {collision.name}");



        // V�rifie si l'objet entrant est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("CHARACTERR ENCONTRE");
            // Applique la force de saut au joueur
            player.ActivateJumpPad(jumpPadForce);
        }
    }
}
