using DG.Tweening;
using System;
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

    private List<GameObject> chatButtons = new();
    private List<GameObject> chats = new();

    private GameObject currentChat;

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
        CreateChat();
        CreateChat();
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

    public void CreateChatButton(int index)
    {
        GameObject chatBtn = Instantiate(chatButtonPrefab, chatButtonContainer);
        chatButtons.Add(chatBtn);
        chatBtn.GetComponent<Button>().onClick.AddListener(() => ShowChat(index));
    }

    public void CreateChat()
    {
        GameObject chat = Instantiate(chatPrefab, chatContainer);
        ChatUI chatUI = chat.GetComponent<ChatUI>();    
        chats.Add(chat);
        chatUI.SetIndex(chats.Count);
        CreateChatButton(chatUI.GetIndex());
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

