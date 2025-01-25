using System.Linq;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    private bool canSpawn = true;
    private MusicManager musicManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicManager = MusicManager.instance;
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
        if (!canSpawn) return;

        GameObject note0 = Instantiate(notePrefab, spawnPoints[0].position, Quaternion.identity);
        GameObject note1 = Instantiate(notePrefab, spawnPoints[1].position, Quaternion.identity);
        GameObject note2 = Instantiate(notePrefab, spawnPoints[2].position, Quaternion.identity);
        GameObject[] notesToSetup = {note0, note1, note2};
        if (musicManager.firstSegment()) {
            MusicStyle[] styles = {MusicStyle.Modern, MusicStyle.Medieval, MusicStyle.SF};
            for (int i = 0; i < notesToSetup.Length; i++)
            {
                notesToSetup[ Random.Range(0, notesToSetup.Length) ].GetComponent<Collectable>().Setup(styles[i], LayerType.Perc);
                Debug.Log($"Spawning note {notesToSetup[i].GetComponent<Collectable>().toString()} in segment first");
                notesToSetup.ToList().RemoveAt(i);
            }
        } else {
            LayerType[] layersStillNone = musicManager.getLayersStillNone();
            MusicStyle percStyle = musicManager.getUnlockedLayers()[2];
            for (int i = 0; i < notesToSetup.Length; i++)
            {
                notesToSetup[ Random.Range(0, notesToSetup.Length) ].GetComponent<Collectable>().Setup(percStyle, layersStillNone[ Random.Range(0, layersStillNone.Length) ]);
                Debug.Log($"Spawning note {notesToSetup[i].GetComponent<Collectable>().toString()} in nth segment");
                notesToSetup.ToList().RemoveAt(i);
            }
        }
        canSpawn = false;
    }
}
