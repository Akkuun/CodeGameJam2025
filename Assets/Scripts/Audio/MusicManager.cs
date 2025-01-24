using System;
using UnityEngine;

enum MusicStyle {
    Modern,
    Medieval,
    SF,
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public float elapsedTime;
    [SerializeField] public AudioSource introAudioSource;
    [SerializeField] public AudioSource[] audioSources;
    [SerializeField] public ScrollManager scrollManager;
    //private float diffSum;
    //private float diffCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.Play();
            audioSource.volume = 0;
        }
        //introAudioSource.Play();
        scrollManager = ScrollManager.instance;
        //diffCount = 0;
        //diffSum = 0;
        elapsedTime = 0;
        Debug.Log("Audio total duration : " + audioSources[0].clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        elapsedTime += Time.deltaTime;
        float thDistance = scrollManager.getTheoreticalDistance(elapsedTime);
        float realDistance = scrollManager.distanceScrolled;
        float diff = thDistance - realDistance;
        */
        //diffCount++;
        //diffSum += diff;
        //audioSources[0].time -= diff;
        // if (diffCount % 100 == 0)
        // {
        //     Debug.Log($"Average diff: {diffSum / diffCount}");
        // }
        //Debug.Log($"Theoretical distance: {thDistance} Real distance: {realDistance} Diff : {diff}");
    }

    public void startDebugTrack() {
        int ind = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[ind].volume = 1;
        
    }
}
