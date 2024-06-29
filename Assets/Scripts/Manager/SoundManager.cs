using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; set; }

    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private AudioSource _backgroundMusicSource;

    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance); Instance = this;

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        _sfxSource = GetComponentInChildren<AudioSource>();
        _musicSource = transform.GetChild(1).GetComponent<AudioSource>();
        _backgroundMusicSource = transform.GetChild(2).GetComponent<AudioSource>();

        SetBackgroundMusicVolume(.1f);
    }

    public void UpdateMusicSlider(Slider slider)
    {
        _backgroundMusicSource.volume = slider.value / 2;
        _musicSource.volume = slider.value;
    }

    public void UpdateSFXSlider(Slider slider)
    {
        _sfxSource.volume = slider.value;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
        _backgroundMusicSource.volume = volume / 10;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        _backgroundMusicSource.volume = volume;
    }

    public void SetMasterVolume(float volume)
    {
        _sfxSource.maxDistance = volume;
        _musicSource.maxDistance = volume;
    }

    public void PlaySFX(string sfx)
    {
        if (sfx == "STOP_SFX")
        {
            _sfxSource.Stop();
            return;
        }

        Sound soundToPlay = sounds.Find(s => s.Name == sfx);

        if (soundToPlay.IsUnityNull()) return;

        if (soundToPlay.Random && soundToPlay.OneShot)
        {
            // Play a random audio clip from MultipleAudio list as a one-shot
            _sfxSource.PlayOneShot(soundToPlay.MultipleAudio[UnityEngine.Random.Range(0, soundToPlay.MultipleAudio.Count)]);
        }
        else if (soundToPlay.Random)
        {
            // Start a coroutine to play multiple random audio clips
            StartCoroutine(PlayMultipleAudio(0.1f, soundToPlay));
        }
        else
        {
            // Play the single audio clip
            _sfxSource.PlayOneShot(soundToPlay.SingleAudio);
        }
    }


    public void PlayMusic(string music)
    {
        if (music == "STOP_MUSIC") { _musicSource.Stop(); }
        sounds.ForEach(s => { if (s.Name == music) _musicSource.PlayOneShot(s.SingleAudio); });
    }

    public void PlayBackgroundMusic(string backgroundMusic)
    {
        if (backgroundMusic == "STOP_MUSIC") { _backgroundMusicSource.Stop(); }
        if (_backgroundMusicSource.isPlaying) { _backgroundMusicSource.Stop(); }
        sounds.ForEach(s => { if (s.Name == backgroundMusic) _backgroundMusicSource.PlayOneShot(s.SingleAudio); });
    }

    private IEnumerator PlayMultipleAudio(float delay, Sound sound)
    {
        List<AudioClip> clipCopyList = sound.MultipleAudio;

        for (int i = 0; i < clipCopyList.Count; i++)
        {
            yield return new WaitForSeconds(delay);

            if (!sound.Random) _sfxSource.PlayOneShot(clipCopyList[i]);

            else
            {
                int randomIndex = UnityEngine.Random.Range(0, clipCopyList.Count);
                _sfxSource.PlayOneShot(clipCopyList[randomIndex]);
            }
        }
    }
}

[Serializable]
public struct Sound
{
    public string Name;
    public AudioClip SingleAudio;

    public List<AudioClip> MultipleAudio;
    public bool Random;
    public bool OneShot;
}
