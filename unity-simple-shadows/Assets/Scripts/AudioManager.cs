using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    public AudioClip backgroundAudioClip;
    public AudioClip selectAudioClip;
    public AudioClip startAudioClip; 

    AudioSource[] audioSources;   

    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;


    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = backgroundAudioClip;
        audioSources[0].Play(); 
    }


    // Toggle if background music is muted.
    // Unmute music when experiment non in session (trial timer not active) 
    public void MuteBackgroundMusic(bool isMuted)
    {
        audioSources[0].mute = isMuted; 
    }

    public void PlaySelectSound()
    {
        audioSources[1].PlayOneShot(selectAudioClip); 
    }

    public void PlayStartSound()
    {
        audioSources[1].PlayOneShot(startAudioClip);
        MuteBackgroundMusic(true); 
    }
   

}
