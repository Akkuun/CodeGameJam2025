using System;
using UnityEngine;

public enum MusicStyle {
    Modern,
    Medieval,
    SF,
    None
}

public enum LayerType {
    Melo1,
    Melo2,
    Perc,
    Acc
}


public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Intro music")]
    [SerializeField] public AudioSource introAudioSource;
    [Header("Music layers")]
    [SerializeField] public AudioSource[] me_audioSources;
    [SerializeField] public AudioSource[] mo_audioSources;
    [SerializeField] public AudioSource[] sf_audioSources;
    [Header("Références")]
    [SerializeField] public ScrollManager scrollManager;
    //private float diffSum;
    //private float diffCount;
    private MusicStyle[] layersUnlocked = new MusicStyle[4]; // 0: Melo1, 1: Melo2, 2: Perc, 3: Acc

    private AudioSource[] getAudioSourceFromStyle(MusicStyle style) {
        if (style == MusicStyle.Modern) {
            return mo_audioSources;
        } else if (style == MusicStyle.Medieval) {
            return me_audioSources;
        } else if (style == MusicStyle.SF) {
            return sf_audioSources;
        }
        return null;
    }

    public void unlockLayer(MusicStyle style, LayerType layer) {
        if (layersUnlocked[(int)layer] == MusicStyle.None) {
            // Set the volume of the new track to 1
            layersUnlocked[(int)layer] = style;
            AudioSource[] audioSources = getAudioSourceFromStyle(style);
            audioSources[(int)layer].volume = 1;
        } else {
            // Remove the volume of the old track and set the volume of the new track to 1
            AudioSource[] audioSources = getAudioSourceFromStyle(layersUnlocked[(int)layer]);
            audioSources[(int)layer].volume = 0;
            layersUnlocked[(int)layer] = style;
            audioSources = getAudioSourceFromStyle(style);
            audioSources[(int)layer].volume = 1;
        }
    }

    private void startSegment(MusicStyle startingStyle) {
        layersUnlocked[2] = startingStyle;
        for (int i = 0; i < 4; i++) {
            if (layersUnlocked[i] == MusicStyle.None) {
                continue;
            } else {
                AudioSource[] audioSources = getAudioSourceFromStyle(layersUnlocked[i]);
                audioSources[i].volume = 1;
            }
        }
        playAll();
    }

    public void startGame(MusicStyle startingStyle) {
        layersUnlocked[0] = MusicStyle.None;
        layersUnlocked[1] = MusicStyle.None;
        layersUnlocked[2] = startingStyle;
        layersUnlocked[3] = MusicStyle.None;
        AudioSource[] audioSources = getAudioSourceFromStyle(startingStyle);
        audioSources[2].volume = 1;
        playAll();
    }

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
        introAudioSource.loop = true;
        introAudioSource.playOnAwake = true;
        foreach(AudioSource audioSource in me_audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 0;
            audioSource.Play();
        }
        foreach(AudioSource audioSource in mo_audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 0;
            audioSource.Play();
        }
        foreach(AudioSource audioSource in sf_audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 0;
            audioSource.Play();
        }
        //introAudioSource.Play();
        scrollManager = ScrollManager.instance;
        Debug.Log("Audio total duration : " + me_audioSources[0].clip.length);
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
        // int ind = UnityEngine.Random.Range(0, me_audioSources.Length);
        // me_audioSources[ind].volume = 1;
        sf_audioSources[0].volume = 1;
        sf_audioSources[1].volume = 1;
        me_audioSources[2].volume = 1;
        mo_audioSources[3].volume = 1;
    }

    public void startIntroTrack() {
        introAudioSource.Play();
    }

    private void playAll() {
        for (int i = 0; i < 4; i++) {
            AudioSource[] audioSources = getAudioSourceFromStyle(layersUnlocked[i]);
            if (layersUnlocked[i] != MusicStyle.None) {
                audioSources[i].volume = 1;
            }
        }
    }
}
