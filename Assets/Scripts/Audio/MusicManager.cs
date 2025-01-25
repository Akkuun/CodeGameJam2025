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

    // 0: Melo1, 1: Melo2, 2: Perc, 3: Acc
    public MusicStyle[] layersUnlocked = new MusicStyle[4]; 

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
            layersUnlocked[(int)layer] = style;
    }

    public void startSegment(MusicStyle percStyle, MusicStyle newLayerStyle, LayerType newLayer) {
        // Starting segment, only the perc layer is changed, with the starting style of the segment
        if (newLayerStyle != MusicStyle.None) {
            layersUnlocked[(int)newLayer] = newLayerStyle;
        } else {
            stopIntroTrack();
        }
        layersUnlocked[2] = percStyle;
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
            audioSource.volume = 1;
        }
        foreach(AudioSource audioSource in mo_audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 1;
        }
        foreach(AudioSource audioSource in sf_audioSources)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 1;
        }
        //introAudioSource.Play();
        scrollManager = ScrollManager.instance;
        layersUnlocked[0] = MusicStyle.None;
        layersUnlocked[1] = MusicStyle.None;
        layersUnlocked[2] = MusicStyle.None;
        layersUnlocked[3] = MusicStyle.None;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startDebugTrack() {
        // Use CTRL + P To execute code here
        stopAll();
    }

    public void startIntroTrack() {
        introAudioSource.Play();
    }

    public void stopIntroTrack() {
        introAudioSource.Stop();
    }

    private void playAll() {
        for (int i = 0; i < 4; i++) {
            if (layersUnlocked[i] != MusicStyle.None) {
                AudioSource[] audioSources = getAudioSourceFromStyle(layersUnlocked[i]);
                audioSources[i].volume = 1;
                audioSources[i].time = 0;
                audioSources[i].Play();
            }
        }
    }

    private void stopAll() {
        for (int i = 0; i < 4; i++) {
            if (layersUnlocked[i] != MusicStyle.None) {
                AudioSource[] audioSources = getAudioSourceFromStyle(layersUnlocked[i]);
                audioSources[i].Stop();
            }
        }
    }

    public void setTheme(MusicStyle style, LayerType type) {
        unlockLayer(style, type);
    }

    public bool firstSegment() {
        return layersUnlocked[0] == MusicStyle.None && layersUnlocked[1] == MusicStyle.None && layersUnlocked[3] == MusicStyle.None;
    }

    public MusicStyle[] getUnlockedLayers() {
        return layersUnlocked;
    }

    public LayerType[] getLayersStillNone() {
        LayerType[] layers = new LayerType[4];
        for (int i = 0; i < 4; i++) {
            if (layersUnlocked[i] == MusicStyle.None) {
                layers[i] = (LayerType)i;
            }
        }
        return layers;
    }
}
