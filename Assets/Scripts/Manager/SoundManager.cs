using System;
using System.Collections;
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
        else Destroy(this);

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
        sounds.ForEach(s => { 
            if (s.Name == sfx) 

                _sfxSource.PlayOneShot(s.SingleAudio);

                if (s.Random && s.OneShot) {_sfxSource.PlayOneShot(s.MultipleAudio[UnityEngine.Random.Range(0, s.MultipleAudio.Count)]); return; }

                if (s.Random) StartCoroutine(PlayMultipleAudio(.1f, s));
        });

    }

    public void PlayMusic(string music)
    {
        sounds.ForEach(s => { if (s.Name == music) _sfxSource.PlayOneShot(s.SingleAudio); });
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
