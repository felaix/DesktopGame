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

    [Header("Beginning")]
    [SerializeField] private GameObject _deviceConfirmation;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
    }


    #region Error Notification
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

        if (ErrorNotificationCounter >= 3)
        {
            TriggerBlueScreen();
        }
    }
    private void DestroyAllErrorNotifications()
    {
        foreach (GameObject obj in _errorNotifications)
        {
            if (obj != null) { Destroy(obj); }
        }

        _errorNotifications.Clear();
    }

    #endregion

    #region Achievements

        public void CreateAchievementUI(Achievement achievementData)
        {
            GameObject achievement = Instantiate(_achievementPrefab, _achievementContainer);
            TMP_Text titleTMP = achievement.transform.GetComponentInChildren<TMP_Text>();
            TMP_Text pointsTMP = achievement.transform.GetChild(1).GetComponent<TMP_Text>();

            titleTMP.text = achievementData.Name;
            pointsTMP.text = achievementData.Points.ToString();
        }

    #endregion

    #region Ending
    public void TriggerBlueScreen()
    {
        _blueScreen.gameObject.SetActive(true);
        DestroyAllErrorNotifications();
    }
    #endregion

    public Canvas GetCanvas() { return _canvas; }
}
