using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] notePrefab;
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    private bool canSpawn = true;
    private bool spawnedNewNotes = false;
    private MusicManager musicManager;
    private GameObject note0;
    private GameObject note1;
    private GameObject note2;
    private ScrollManager gameManager;
    private int loop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicManager = MusicManager.instance;
        gameManager = ScrollManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void resetSpawnCondition() {
        canSpawn = true;
    }
    public void spawnNote()
    {
        Debug.Log($"Spawning note : {canSpawn}, first segment : {musicManager.firstSegment()}");
        //if (!canSpawn) return;

        note0 = Instantiate(notePrefab[0], spawnPoints[0].position, Quaternion.identity);
        note1 = Instantiate(notePrefab[1], spawnPoints[1].position, Quaternion.identity);
        note2 = Instantiate(notePrefab[2], spawnPoints[2].position, Quaternion.identity);
        
        MusicStyle[] styles = {MusicStyle.Modern, MusicStyle.Medieval, MusicStyle.SF};
        LayerType[] layers;
        if (musicManager.layersUnlocked[0] == MusicStyle.None && musicManager.layersUnlocked[1] == MusicStyle.None && musicManager.layersUnlocked[3] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo1, LayerType.Melo2, LayerType.Acc};
        } else if (musicManager.layersUnlocked[0] == MusicStyle.None && musicManager.layersUnlocked[1] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo1, LayerType.Melo2};
        } else if (musicManager.layersUnlocked[0] == MusicStyle.None && musicManager.layersUnlocked[3] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo1, LayerType.Acc};
        } else if (musicManager.layersUnlocked[1] == MusicStyle.None && musicManager.layersUnlocked[3] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo2, LayerType.Acc};
        } else if (musicManager.layersUnlocked[0] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo1};
        } else if (musicManager.layersUnlocked[1] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Melo2};
        } else if (musicManager.layersUnlocked[3] == MusicStyle.None) {
            layers = new LayerType[] {LayerType.Acc};
        } else {
            layers = new LayerType[] {LayerType.Melo1, LayerType.Melo2, LayerType.Acc};
        }

        note0.GetComponent<Collectable>().Setup(styles[ Random.Range(0, styles.Length) ], layers[ Random.Range(0, layers.Length) ]);
        note1.GetComponent<Collectable>().Setup(styles[ Random.Range(0, styles.Length) ], layers[ Random.Range(0, layers.Length) ]);
        note2.GetComponent<Collectable>().Setup(styles[ Random.Range(0, styles.Length) ], layers[ Random.Range(0, layers.Length) ]);

        // GameObject[] notesToSetup = {note0, note1, note2};
        // if (musicManager.firstSegment()) {
        //     MusicStyle[] styles = {MusicStyle.Modern, MusicStyle.Medieval, MusicStyle.SF};
        //     for (int i = 0; i < notesToSetup.Length; i++)
        //     {
        //         notesToSetup[ Random.Range(0, notesToSetup.Length) ].GetComponent<Collectable>().Setup(styles[i], LayerType.Perc);
        //         Debug.Log($"Spawning note {notesToSetup[i].GetComponent<Collectable>().toString()} in segment first");
        //         notesToSetup.ToList().RemoveAt(i);
        //     }
        // } else {
        //     LayerType[] layersStillNone = musicManager.getLayersStillNone();
        //     MusicStyle percStyle = musicManager.getUnlockedLayers()[2];
        //     for (int i = 0; i < notesToSetup.Length; i++)
        //     {
        //         notesToSetup[ Random.Range(0, notesToSetup.Length) ].GetComponent<Collectable>().Setup(percStyle, layersStillNone[ Random.Range(0, layersStillNone.Length) ]);
        //         Debug.Log($"Spawning note {notesToSetup[i].GetComponent<Collectable>().toString()} in nth segment");
        //         notesToSetup.ToList().RemoveAt(i);
        //     }
        //}
        //canSpawn = false;
    }

    public void destroyNotes() {
        if (note0 != null) {
            Destroy(note0);
            note0 = null;
        }
        if (note1 != null) {
            Destroy(note1);
            note1 = null;
        }
        if (note2 != null) {
            Destroy(note2);
            note2 = null;
        }
    }

    public bool canNoteBeDestroyed() {
        return note0 != null || note1 != null || note2 != null;
    }
}
