using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseButton : MonoBehaviour
{
    public int index;

    private Button btn;
    private Transform container;
    private ChatUI chat;

    public void SetChat(ChatUI c) { chat = c; }
    public ChatUI GetChat() { return chat; }

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Respond);

        container = GetComponentInParent<Transform>();
    }

    private void FixedUpdate()
    {
        btn.interactable = DialogueManager.Instance.GetCanAnswer();
    }

    public void Respond()
    {
        container.gameObject.SetActive(false);
        chat.Respond(index);
        //DialogueManager.Instance.OnRespond(index);

        //if (gameObject != null) Destroy(gameObject, .1f);
        //Debugger.Instance.CreateLog("Choice Clicked");
    }
}
