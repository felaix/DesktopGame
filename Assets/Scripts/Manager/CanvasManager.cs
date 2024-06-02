using EditorAttributes;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    private Canvas _canvas;
    [ReadOnly] public int ErrorNotificationCounter = 1;
    [ReadOnly] public List<GameObject> _errorNotifications;


    public GameObject _blueScreen;


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


    private void TriggerBlueScreen()
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

    public Canvas GetCanvas() { return _canvas; }
}
