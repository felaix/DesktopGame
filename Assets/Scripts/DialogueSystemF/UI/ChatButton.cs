using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatButton : MonoBehaviour
{
    private TMP_Text statusTMP;
    private TMP_Text usernameTMP;
    private Image profilePicture;
    private GameObject notification;
    private TMP_Text notificationTMP;

    public string Status;
    public string Username;
    public Sprite ProfilePicture;

    public int unreadMessages = 0;

    private void GetComponents()
    {
        statusTMP = transform.GetChild(1).GetComponent<TMP_Text>();
        usernameTMP = GetComponentInChildren<TMP_Text>();
        profilePicture = GetComponentInChildren<Image>();
        notification = transform.GetChild(3).gameObject;
        notificationTMP = notification.GetComponentInChildren<TMP_Text>();
    }

    private void ReloadNotificationTMP() => notificationTMP.text = unreadMessages.ToString();
    public void ResetUnreadMessages() { unreadMessages = 0; HideNotification(); ReloadNotificationTMP(); }
    public void IncreaseUnreadMessages() { unreadMessages++; ShowNotification(); ReloadNotificationTMP(); }
    public void ShowNotification() => notification.SetActive(true);
    public void HideNotification() => notification.SetActive(false);

    public void Initialize(Sprite sprite = null, string status = "online", string username = "username")
    {
        GetComponents();
        SetStatus(status);
        SetUsername(username);
        SetStatus(status);
        if (sprite != null) SetProfilePicture(sprite);
    }

    public void SetStatus(string status)
    {
        statusTMP.text = status;
        Status = status;
    }

    public void SetUsername(string username)
    {
        usernameTMP.text = username;
        Username = username;
    }

    public void SetProfilePicture(Sprite sprite)
    {
        if (sprite == null) return;
        profilePicture.sprite = sprite;
        ProfilePicture = sprite;
    }

}

