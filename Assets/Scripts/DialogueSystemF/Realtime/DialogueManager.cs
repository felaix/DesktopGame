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

    private List<GameObject> chatButtons = new();
    private List<GameObject> chats = new();

    private GameObject currentChat;

    public List<DialogueBaseNodeSO> dialogues = new();  
    private bool canAnswer = false;
    public bool GetCanAnswer() => canAnswer;
    public void SetCanAnswer(bool can) => canAnswer = can;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

        CreateChat(dialogues[0], "David");
        
    }


    //public void Reload(DialogueBaseNodeSO dialogue, int index)
    //{
    //    Debug.Log("Reload");
    //    DeleteChoices();
    //    SetNewDialogue(dialogue);
    //    //CreateChoiceButtons(dialogue);
    //    if (currentDialogue.SkipChoices()) { SetNewDialogue(currentDialogue.NextNode); StartCoroutine(NPCNextMessageCoroutine()); }
    //    else ActivateButtons();
    //}

    public void CreateChatButton(int index, string name)
    {
        GameObject chatBtn = Instantiate(chatButtonPrefab, chatButtonContainer);
        chatButtons.Add(chatBtn);
        chatBtn.GetComponentInChildren<TMP_Text>().text = name;
        chatBtn.GetComponent<Button>().onClick.AddListener(() => ShowChat(index));
    }

    public void CreateChat(DialogueBaseNodeSO dialogue, string name)
    {
        GameObject chat = Instantiate(chatPrefab, chatContainer);
        ChatUI chatUI = chat.GetComponent<ChatUI>();
        chats.Add(chat);
        chatUI.SetIndex(chats.Count);
        chatUI.dialogueSO = dialogue;
        CreateChatButton(chatUI.GetIndex(), name);
    }

    public void ShowChat(int index)
    {
        Debug.Log("index: "+ index);
        Debug.Log("SHow chat");
        if (currentChat == chats[index-1]) return;
        if (currentChat != null) currentChat.SetActive(false);
        chats[index-1].SetActive(true);
        currentChat = chats[index - 1];
    }

}

