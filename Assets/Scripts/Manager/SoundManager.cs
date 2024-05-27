using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; set; }

    private AudioSource _sfxSource;
    private AudioSource _musicSource;

    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        _sfxSource = GetComponentInChildren<AudioSource>();
        _musicSource = transform.GetChild(1).GetComponent<AudioSource>();
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SetMasterVolume(float volume)
    {
        _sfxSource.maxDistance = volume;
        _musicSource.maxDistance = volume;
    }

    public void PlaySFX(string sfx)
    {
        sounds.ForEach(s => { if (s.Name == sfx) _sfxSource.PlayOneShot(s.Audio); });
    }

    public void PlayMusic(string music)
    {
        sounds.ForEach(s => { if (s.Name == music) _sfxSource.PlayOneShot(s.Audio); });
    }
}

[Serializable]
public struct Sound
{
    public string Name;
    public AudioClip Audio;
}
