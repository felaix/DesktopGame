using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicApp : MonoBehaviour
{
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button toggleBtn;

    [SerializeField] private Image coverImage;
    [SerializeField] private TMP_Text songTitle;

    [ReadOnly] private int currentIndex = 0;
    [SerializeField] private List<AppSongData> songs = new();


    private bool toggled = false;

    private void Awake()
    {
        previousBtn.onClick.AddListener(() => PlayPreviousSong());
        nextBtn.onClick.AddListener(() => PlayNextSong());
        toggleBtn.onClick.AddListener(() => PlayToggle());
    }

    private void PlayPreviousSong()
    {
        //! TODO : Change UI


        // Set Index
        currentIndex--;
        if (currentIndex < 0) { currentIndex = 0; }

        // Play BG Music
        SoundManager.Instance.PlayBackgroundMusic(songs[currentIndex].CodeName);

        // Reload / Update UI
        coverImage.sprite = songs[currentIndex].Cover;
        songTitle.text = songs[currentIndex].Title;
    }

    private void PlayNextSong()
    {
        // TODO : Change UI

        // Set Index
        currentIndex++;
        int songsCount = songs.Count - 1;
        if (currentIndex > songsCount) { currentIndex = songsCount; }

        // Play BG Music
        SoundManager.Instance.PlayBackgroundMusic(songs[currentIndex].CodeName);

        // Reload / Update UI
        coverImage.sprite = songs[currentIndex].Cover;
        songTitle.text = songs[currentIndex].Title;
    }

    private void PlayToggle()
    {

        // Change Button Sprite


        // Toggle Backgroundmusic
        if (!toggled)
        {
            SoundManager.Instance.PlayBackgroundMusic("STOP_Music");
            toggled = true;
        }
        else
        {
            SoundManager.Instance.PlayBackgroundMusic(songs[currentIndex].CodeName);
            toggled = false;
        }
    }

}

[Serializable]
public class AppSongData
{
    public string CodeName;
    public Sprite Cover;
    public string Title;
}