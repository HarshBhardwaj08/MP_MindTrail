using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{get;private set;}
    public AudioSource musicSource;         // Audio source for background music
    public AudioSource sfxSource;           // Audio source for sound effects
    public AudioClip[] musicClips;          // List of background music clips
    public AudioClip[] sfxClips;            // List of sound effect clips

    private Dictionary<string, AudioClip> sfxDict;  // Dictionary to store SFX by name
    private void OnEnable()
    {
        FlowChanger.flowDelegate += ChangeVector;
        PlayerController.OnPlayerJump += PlaySfx;
        Interactables.OnPlayerInteraction += PlaySfx;
    }

    private void PlaySfx(int obj)
    {
        sfxSource.PlayOneShot(sfxClips[obj]);
    }
    private void OnDisable()
    {
        FlowChanger.flowDelegate -= ChangeVector;
        PlayerController.OnPlayerJump -= PlaySfx;
        Interactables.OnPlayerInteraction -= PlaySfx;
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            Instance = this;
        }

    }
    private void ChangeVector(float camDis, int musicIndex)
    {
        PlayMusic(musicIndex);
    }


    private void Awake()
    {
        // Initialize the SFX dictionary
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in sfxClips)
        {
            sfxDict.Add(clip.name, clip);  // Store each SFX with its name
        }
    }



    // Play background music by index
    private void PlayMusic(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicClips.Length)
        {
            musicSource.clip = musicClips[trackIndex];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Track index out of range!");
        }
    }

    // Play sound effect by name
    private void PlaySFX(string sfxName)
    {
        if (sfxDict.ContainsKey(sfxName))
        {
            sfxSource.PlayOneShot(sfxDict[sfxName]);
        }
        else
        {
            Debug.LogWarning("SFX name not found!");
        }
    }

    // Adjust the volume of the background music
    private void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Adjust the volume of sound effects
    private void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // Stop the current background music
    private void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffects(int num)
    {
        sfxSource.PlayOneShot(sfxClips[num]);
    }
    
}
