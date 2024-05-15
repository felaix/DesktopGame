using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float timeScale = 60f;
    private DateTime currentTime;

    private void Awake()
    {
        Instance = this;

        currentTime = DateTime.Now;
    }

    private void Update()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeScale);
    }

    public string GetTime() => currentTime.ToString("hh:mm tt").ToLower();
}
