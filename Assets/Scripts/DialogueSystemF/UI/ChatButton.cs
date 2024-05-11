using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatButton : MonoBehaviour
{
    private TMP_Text statusTMP;
    private TMP_Text usernameTMP;
    private Image profilePicture;

    public string Status;
    public string Username;
    public Sprite ProfilePicture;

    private void GetComponents()
    {
        statusTMP = transform.GetChild(1).GetComponent<TMP_Text>();
        usernameTMP = GetComponentInChildren<TMP_Text>();
        profilePicture = GetComponentInChildren<Image>();
    }

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

