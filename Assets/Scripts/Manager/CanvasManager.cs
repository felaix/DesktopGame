using EditorAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    private Canvas _canvas;
    [ReadOnly] public int ErrorNotificationCounter = 1;
    [ReadOnly] public List<GameObject> _errorNotifications;

    [Header("Ending")]
    [SerializeField] private GameObject _blueScreen;
    [SerializeField] private Transform _achievementContainer;
    [SerializeField] private GameObject _achievementPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
    }

    public void IncreaseErrorNotificationCounter(List<GameObject> notifications)
    {
        ErrorNotificationCounter++;

        foreach (GameObject notification in notifications)
        {
            if (notification != null)
            {
                _errorNotifications.Add(notification);
            }
        }

        if (ErrorNotificationCounter == 4)
        {
            TriggerBlueScreen();
        }
    }


    public void TriggerBlueScreen()
    {
        _blueScreen.gameObject.SetActive(true);
        DestroyAllErrorNotifications();
    }

    private void DestroyAllErrorNotifications()
    {
        foreach (GameObject obj in _errorNotifications)
        {
            if (obj != null) { Destroy(obj); }
        }

        _errorNotifications.Clear();
    }

    public void CreateAchievement(string text, float value, float sliderMaxValue = 5f)
    {
        GameObject achievement = Instantiate(_achievementPrefab, _achievementContainer);
        TMP_Text tmp = achievement.GetComponentInChildren<TMP_Text>();
        Slider slider = achievement.GetComponentInChildren<Slider>();

        tmp.text = text;
        slider.maxValue = sliderMaxValue;
        slider.minValue = 0f;
        slider.value = value;
    }

    public Canvas GetCanvas() { return _canvas; }
}
