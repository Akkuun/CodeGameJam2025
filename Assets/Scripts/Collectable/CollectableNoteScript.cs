using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public MusicStyle musicStyle;
    [Header("Layer type doesn't matter for first note.")]
    [Header("Also don't use layer type Perc")]
    [SerializeField] public LayerType layerType;
    private ScrollManager gameManager;
    void Start()
    {
        gameManager = ScrollManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            gameManager.startSegment(this);
            Destroy(gameObject);
        }
    }
}
