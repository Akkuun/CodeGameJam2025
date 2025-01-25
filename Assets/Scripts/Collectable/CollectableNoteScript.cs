using System;
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

    void Update()
    {
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            Debug.Log("Changing music");
            gameManager.gameState = GameState.Game;
            gameManager.startSegment(this);
            DestroyAndResetSpawnCondition();
        }
    }

    private void DestroyAndResetSpawnCondition()
    {
        Destroy(gameObject);
        gameManager.noteSpawner.resetSpawnCondition();
    }

    public void Setup(MusicStyle musicStyle, LayerType layerType)
    {
        this.musicStyle = musicStyle;
        this.layerType = layerType;
    }


    public string toString()
    {
        return $"Collectable Note = MusicStyle: {musicStyle}, LayerType: {layerType}";
    }
}
