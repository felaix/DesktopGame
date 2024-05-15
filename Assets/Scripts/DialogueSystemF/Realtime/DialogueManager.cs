using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Chat UI ")]
    [SerializeField] private GameObject chatButtonPrefab;
    [SerializeField] private GameObject chatPrefab;

    [SerializeField] private Transform chatButtonContainer;
    [SerializeField] private Transform chatContainer;

    private List<ChatButton> chatButtons = new();
    private List<GameObject> chats = new();

    private GameObject currentChat;

    public List<DialogueBaseNodeSO> dialogues = new();  
    private bool canAnswer = false;
    public bool GetCanAnswer() => canAnswer;
    public void SetCanAnswer(bool can) => canAnswer = can;
    public void SetNotification(int index) { if (currentChat == chats[index]) return; else chatButtons[index].IncreaseUnreadMessages(); }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

        //dialogues.ForEach(dialogue => CreateChat(dialogue, dialogue.npc.ToString()));

        StartCoroutine(StartChat());

        //CreateChat(dialogues[0], "David");
    }

    private IEnumerator StartChat()
    {
        yield return new WaitForSeconds(1f);

        while (dialogues.Count > 0)
        {
            Debug.Log("Create Chat");
            CreateChat(dialogues[0], dialogues[0].npc.ToString());
            dialogues.RemoveAt(0);
            yield return new WaitForSeconds(20f);
        }
    }

    public void CreateChatButton(int index, string name)
    {
        ChatButton chatBtn = Instantiate(chatButtonPrefab, chatButtonContainer).GetComponent<ChatButton>();
        chatButtons.Add(chatBtn);
        chatBtn.Initialize(null,"last seen...", name);

        chatBtn.GetComponent<Button>().onClick.AddListener(() => ShowChat(index));
    }

    public void SetChatStatus(int index, string status) => chatButtons[index].SetStatus(status);
    public void SetChatNotification(int index) => chatButtons[index].unreadMessages++;

    public void AddChat(GameObject chat, bool createBtn = false)
    {
        chats.Add(chat);
        ChatUI chatUI = chat.GetComponent<ChatUI>();
        chatUI.SetIndex(chats.Count);

        if (createBtn) CreateChatButton(chatUI.GetIndex(), chatUI.dialogueSO.npc.ToString());
    }

    public void CreateChat(DialogueBaseNodeSO dialogue, string name)
    {
        GameObject chat = Instantiate(chatPrefab, chatContainer);
        ChatUI chatUI = chat.GetComponent<ChatUI>();
        AddChat(chat);
        //chats.Add(chat);
        //chatUI.SetIndex(chats.Count);
        chatUI.dialogueSO = dialogue;
        CreateChatButton(chatUI.GetIndex(), name);
    }

    public void ShowChat(int index)
    {
        //if (currentChat == chats[index-1]) { currentChat.transform.GetChild(0).gameObject.SetActive(true); return; }

        if (currentChat != null) currentChat.transform.GetChild(0).gameObject.SetActive(false);
        
        chats[index - 1].transform.GetChild(0).gameObject.SetActive(true);
        currentChat = chats[index - 1];

        SelectChat(index - 1);
    }

    public void SelectChat(int index)
    {

        Debug.Log("Selecting chat " + index);

        ColorBlock selectedColors = new ColorBlock
        {
            normalColor = new Color32(45, 45, 48, 200),
            highlightedColor = new Color32(45, 45, 48, 200),
            pressedColor = new Color32(62, 62, 66, 200),
            disabledColor = new Color32(200,200,200, 200),
            selectedColor = new Color32(45,45,48, 200),
            colorMultiplier = 1f,
            fadeDuration = .1f
        };

        ColorBlock normalColors = new ColorBlock
        {
            normalColor = new Color32(37, 37, 38, 200),
            highlightedColor = new Color32(45, 45, 48, 200),
            pressedColor = new Color32(62, 62, 66, 200),
            disabledColor = new Color32(200, 200, 200, 200),
            selectedColor = new Color32(45, 45, 48, 200),
            colorMultiplier = 1f,
            fadeDuration = .1f
        };

        foreach (var ch in chatButtons)
        {
            ch.GetComponent<Button>().colors = normalColors;
        }

        chatButtons[index].GetComponent<Button>().colors = selectedColors;
        chatButtons[index].ResetUnreadMessages();

    }

}

