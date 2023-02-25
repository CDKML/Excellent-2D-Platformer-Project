using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

/**
  This should be a singleton, so it is common for all scenes.
  It will play the audio loops in queue providing a continuous sound stream
  trough the game but allowing to switch between loops for different scenes or 
  game events.

  When we want to change the music, we call the method ChangeMusic() and pass
  a list of the new Theme clips. Optionally pass a fillClip to make a smoother
  transition between themes.

  Use method Loop() to set the loop mode on or off. Deafults ON.

  Event onLastClip is called when the last clip of the list is loaded for 
  event sequencing purposes.

  Event onLoopEnd is called when the last clip of the loop list is endend.

  Event onPlayEnd is called when the last clip of the list is endend.
*/

public class MusicManager : MonoBehaviour
{

    public static GameObject musicManagerObject = null;
    public AudioMixerGroup musicMixerGroup;
    public List<AudioClip> defaultMusicClips;
    public List<AudioClip> audioClipList;
    public UnityEvent onLastClip;
    public UnityEvent onLoopEnd;
    public UnityEvent onPlayEnd;

    private AudioSource musicSource;
    private AudioSource musicFillSource;
    private AudioSource[] audioSourceArray;
    private string scheduledClip; 
    private string lastLoopClip;   
    private double nextStartTime;
    private int toggle = 0;
    private int clipsInLoop = 1;
    [SerializeField]
    private bool doLoop = true;
    private bool isPlaying = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(musicManagerObject == null){
            musicManagerObject = this.gameObject;
        } else if(musicManagerObject != null){
            Destroy(this.gameObject);
        }        
    }

    void Start()
    {
        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicFillSource = gameObject.AddComponent<AudioSource>();
        audioSourceArray = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSourceArray)
        {
            source.outputAudioMixerGroup = musicMixerGroup;
        }

        if(defaultMusicClips.Count == 0 && audioClipList.Count == 0)
        {
            Debug.LogError("ERROR: No music clips implemented for MusicManager.");
        } 
        else if (defaultMusicClips.Count != 0 && audioClipList.Count == 0)
        {
            clipsInLoop = defaultMusicClips.Count;
            lastLoopClip = defaultMusicClips[clipsInLoop-1].name;

            foreach (AudioClip clip in defaultMusicClips)
            {
                audioClipList.Add(clip);
            }
        } 
        else if (audioClipList.Count > 0)
        {
            Debug.Log("Clips ready to play.");
        } else {
            Debug.LogError("ERROR: Something went wrong with the music clips.");
        }

        nextStartTime = AudioSettings.dspTime + 0.2;
    }

    void Update()
    {
        if(AudioSettings.dspTime > nextStartTime - 0.5)
        {
            if (audioClipList.Count > 0 ){
                if (!isPlaying){
                      isPlaying = true;
                      nextStartTime = AudioSettings.dspTime + 0.2;
                }

                AudioClip clipToPlay = audioClipList[0];
                scheduledClip = clipToPlay.name;

                audioSourceArray[toggle].clip = clipToPlay;
                audioSourceArray[toggle].PlayScheduled(nextStartTime);

                double duration = (double)clipToPlay.samples / clipToPlay.frequency;
                nextStartTime = nextStartTime + duration;

                toggle = 1 - toggle;

                if (audioClipList.Count <= clipsInLoop && doLoop) {
                    audioClipList.Add(audioClipList[0]);
                }

                audioClipList.RemoveAt(0);

                if (scheduledClip == lastLoopClip && onLoopEnd != null){
                    onLoopEnd.Invoke();
                } 
            }

            if (audioClipList.Count == 0 && isPlaying){
                if (onLastClip != null) onLastClip.Invoke();
            }

        } else if (AudioSettings.dspTime >= nextStartTime)
        {
            isPlaying = false;
            if (onPlayEnd != null) onPlayEnd.Invoke();
        }
    }

    public void ChangeMusic(List<AudioClip> musica, AudioClip musicaFill = null)
    {   
        audioClipList.Clear();

        clipsInLoop = musica.Count;
        lastLoopClip = musica[clipsInLoop-1].name;

        if (musicaFill != null)
        {
            audioClipList.Add(musicaFill);
        }

        foreach (AudioClip clip in musica)
        {
            audioClipList.Add(clip);
        }
    }

    public void Loop(bool doLoop)
    {
        this.doLoop = doLoop;
    }
}