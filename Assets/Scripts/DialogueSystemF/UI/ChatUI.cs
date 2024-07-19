using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject npcMessagePrefab;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Container")]
    [SerializeField] private Transform npcMessageContainer;
    [SerializeField] private Transform choiceButtonContainer;
    [SerializeField] private Transform userMessageContainer;

    public DialogueBaseNodeSO dialogueSO;
    
    public bool readOnly = false;

    // ---- _

    private int chatIndex;

    private List<GameObject> choiceButtons = new();

    // --- Coroutines ---
    private Coroutine userMessageCoroutine;
    private Coroutine npcMessageCoroutine;
    private Coroutine npcNextMessageCoroutine;

    private RectTransform savedUserRect;
    private RectTransform savedNpcRect;

    // --- TMPs ---
    private TMP_Text savedUserTimeTMP;
    private TMP_Text savedNPCMsgTMP;
    private TMP_Text savedNPCTimeTMP;
    private string originalTxt;

    // --- Saving tuples ---
    private (TMP_Text, string) savedNPCMessage;

    private void Start()
    {
        if (readOnly) { DialogueManager.Instance.AddChat(gameObject, true); return; }

        CreateNPCMessage();
        CreateChoiceButtons();
    }
    private void OnEnable()
    {
        if (userMessageCoroutine != null)
        {
            StartCoroutine(UserMessageSpawnCoroutine(savedUserRect, savedUserTimeTMP));
            userMessageCoroutine = null;
        }

        if (npcMessageCoroutine != null)
        {
            StartCoroutine(NPCMessageSpawnCoroutine(savedNpcRect, savedNPCMsgTMP, savedNPCTimeTMP, originalTxt));
            npcMessageCoroutine = null;
        }

        if (npcNextMessageCoroutine != null)
        {
            StartCoroutine(NPCNextMessageCoroutine());
            npcNextMessageCoroutine = null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetNewDialogue(DialogueBaseNodeSO newDialogue) => dialogueSO = newDialogue;
    public int GetIndex() => chatIndex;
    public void SetIndex(int index) { chatIndex = index; }
    private void SetTMPText(TMP_Text tmp, string txt) { if (tmp != null) tmp.text = txt; }
    public void Respond(int index)
    {
        //Debug.Log("Respond index: " + index);

        CreateUserMessage(index);
        SetNewDialogue(dialogueSO.Choices[index].NextDialogue);
        DeleteChoices();

        if (dialogueSO == null) return;
        CreateNPCMessage();
        CreateChoiceButtons();


        if (dialogueSO.SkipChoices()) { SetNewDialogue(dialogueSO.NextNode); npcNextMessageCoroutine = StartCoroutine(NPCNextMessageCoroutine()); }
    }

    #region Choices
    private void ActivateChoiceButtons() => choiceButtons.ForEach(button => { button.SetActive(true); });
    public void DeleteChoices()
    {
        choiceButtons.ForEach(button => { Destroy(button); });
        choiceButtons.Clear();
    }

    #endregion

    #region Create

    private void ConvertUserName(TMP_Text tmp, string originalTxt)
    {
        tmp.text = originalTxt;
        string username = Environment.UserName;
        string newTxt = originalTxt.Replace("user", username);
        tmp.text = newTxt;
    }

    public void CreateNPCMessage()
    {

        // Instantiate NPC Message
        GameObject npcMessageInstance = Instantiate(npcMessagePrefab, npcMessageContainer);

        // Set Scale to 0 for animation
        npcMessageInstance.transform.localScale = Vector3.zero;

        // Set & Save Time TMP
        TMP_Text timeTMP = npcMessageInstance.transform.GetChild(2).GetComponent<TMP_Text>();
        savedNPCTimeTMP = timeTMP;

        // Set & Save Dialogue TMP
        TMP_Text dialogueTMP = npcMessageInstance.GetComponentInChildren<TMP_Text>();
        dialogueTMP.text = dialogueSO.Dialogue;
        savedNPCMsgTMP = dialogueTMP;
        if (dialogueSO.Dialogue.Contains("user")) { Debug.Log("contains user."); ConvertUserName(dialogueTMP, dialogueSO.Dialogue); }

        // Set Image in case the message contains a photo.
        Image img = npcMessageInstance.transform.GetChild(0).GetComponent<Image>();
        img.sprite = dialogueSO.PhotoToSend;

        // On Start Action
        ActionInvoker.Instance.InvokeStartEvent(dialogueSO.OnStartAction);

        // On Click Action
        Button btn = npcMessageInstance.AddComponent<Button>();
        ActionInvoker.Instance.SaveButtonEvent(btn, dialogueSO.OnClickAction);

        btn.onClick.AddListener(() => 
        {
            ActionInvoker.Instance.InvokeButton(btn);
            //ActionInvoker.Instance.InvokeOnClickEvent(btn, invoked, dialogueSO.OnClickAction, img.transform);
        });

        // Trigger Sound
        if (dialogueSO.MusicSound != "") SoundManager.Instance.PlayMusic(dialogueSO.MusicSound);
        if (dialogueSO.SFXSound != "") SoundManager.Instance.PlaySFX(dialogueSO.SFXSound);

        // Animate Message
        npcMessageCoroutine = StartCoroutine(NPCMessageSpawnCoroutine((RectTransform)npcMessageInstance.transform, dialogueTMP, timeTMP, dialogueTMP.text, img));

        // Trigger a new chat
        if (dialogueSO.TriggerDialogue != null) DialogueManager.Instance.CreateChat(dialogueSO.TriggerDialogue, dialogueSO.GetTriggerNPCName());
    }
    public void CreateChoiceButtons()
    {
        if (dialogueSO.SkipChoices()) return;

        for (int i = 0; i < dialogueSO.Choices.Count; i++)
        {
            GameObject choiceInstance = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            choiceButtons.Add(choiceInstance);

            DialogueResponseButton dialogueResponseButton = choiceInstance.GetComponent<DialogueResponseButton>();
            dialogueResponseButton.index = i;
            dialogueResponseButton.SetChat(this);

            TMP_Text responseTMP = choiceInstance.GetComponentInChildren<TMP_Text>();
            //Debug.Log(dialogueSO.GetChoiceText(i));
            responseTMP.text = dialogueSO.GetChoiceText(i);
            
            choiceInstance.SetActive(false);
        }
    }
    public void CreateUserMessage(int choice)
    {
        if (dialogueSO.SkipChoices()) return;

        if (dialogueSO.Choices[choice].ChoiceText == "(say nothing)") return;

        // Instantiate Message
        GameObject userMessageInstance = Instantiate(userMessagePrefab, userMessageContainer);

        RectTransform userMsgRect = userMessageInstance.GetComponent<RectTransform>();
        userMsgRect.transform.localPosition = Vector3.zero;
        userMsgRect.localScale = new Vector3(1, 0, 0);

        TMP_Text userTimeTMP = userMsgRect.GetChild(1).GetComponent<TMP_Text>();
        userTimeTMP.text = TimeManager.Instance.GetTime();
        savedUserTimeTMP = userTimeTMP;

        // Set Text
        TMP_Text userTMP = userMessageInstance.GetComponentInChildren<TMP_Text>();
        userTMP.text = dialogueSO.GetChoiceText(choice);

        GameManager.Instance.AddPlayerTrait(dialogueSO.GetTrait(choice));  

        // Ensure the layout updates after adding a new message
        //LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());

        // Animate Message
        userMessageCoroutine = StartCoroutine(UserMessageSpawnCoroutine(userMsgRect, userTimeTMP));
    }

    #endregion

    #region Coroutines

    private IEnumerator NPCNextMessageCoroutine()
    {
        yield return new WaitForSeconds(dialogueSO.Delay + 2f); // ! MINIMUM 2 SECONDS 
        CreateNPCMessage();

        if (dialogueSO.SkipChoices()) { SetNewDialogue(dialogueSO.NextNode); npcNextMessageCoroutine = StartCoroutine(NPCNextMessageCoroutine()); Debug.Log("Start next msg coroutine"); }
        else { CreateChoiceButtons(); }
    }

    private IEnumerator UserMessageSpawnCoroutine(RectTransform t, TMP_Text timeTMP)
    {
        savedUserRect = t;

        t.DOScale(new Vector3(1, 1, 1), .35f).SetEase(Ease.InOutElastic);

        //t.DOScaleX(0f, .0f);
        //t.DOLo(1f, .35f);
        //t.DOLocalMoveY(1f, 1f);

        timeTMP.text = TimeManager.Instance.GetTime();

        yield return new WaitForSeconds(.25f);

        userMessageCoroutine = null;
        yield return null;
    }
    private IEnumerator NPCMessageSpawnCoroutine(RectTransform t, TMP_Text msgTMP, TMP_Text timeTMP, string txt = "", Image img = null)
    {
        savedNpcRect = t;
        if (txt == "") originalTxt = msgTMP.text;
        else originalTxt = txt;

        DialogueManager.Instance.SetCanAnswer(false);
        DialogueManager.Instance.SetChatStatus(this.GetIndex() - 1, "typing...");

        while (!DialogueManager.Instance.GetCanAnswer())
        {
            SetTMPText(timeTMP, "");
            t.DOScale(new Vector3(0f, 1f, 1f), .25f);
            yield return new WaitForSeconds(.5f);
            SetTMPText(msgTMP, "");
            t.DOScaleX(.5f, .25f);
            SetTMPText(msgTMP, ". . .");
            yield return new WaitForSeconds(.5f);
            SetTMPText(msgTMP, ". . .");

            yield return new WaitForSeconds(.12f);
            SetTMPText(msgTMP, ". .");

            yield return new WaitForSeconds(.14f);
            SetTMPText(msgTMP, ".");
            yield return new WaitForSeconds(.1f);
            SetTMPText(msgTMP, ".");

            yield return new WaitForSeconds(.1f);
            SetTMPText(msgTMP, ". .");
            yield return new WaitForSeconds(.11f);
            SetTMPText(msgTMP, ". . .");

            yield return new WaitForSeconds(.17f);

            SetTMPText(timeTMP, TimeManager.Instance.GetTime());
            DialogueManager.Instance.SetCanAnswer(true);
        }

        t.DOScale(new Vector3(1f,1f,1f), .5f);
        SetTMPText(msgTMP, originalTxt);


        if (img.sprite != null)
        {
            //imgComp.sprite = dialogueSO.PhotoToSend;
            img.gameObject.SetActive(true);
        }
        else { img.gameObject.SetActive(false); }

        DialogueManager.Instance.SetNotification(this.GetIndex() - 1);
        DialogueManager.Instance.SetChatStatus(this.GetIndex() - 1, "online");
        
        if (!dialogueSO.SkipChoices()) ActivateChoiceButtons();

        yield return new WaitForSeconds(1f);
        npcMessageCoroutine = null;
    }


    #endregion


}
