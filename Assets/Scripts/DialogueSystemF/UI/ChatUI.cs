using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private GameObject npcMessagePrefab;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject choiceButtonPrefab; 

    [SerializeField] private Transform npcMessageContainer;
    [SerializeField] private Transform choiceButtonContainer;
    [SerializeField] private Transform userMessageContainer;
    private int chatIndex;
    public DialogueBaseNodeSO dialogueSO;

    private List<GameObject> choiceButtons = new();

    private Coroutine userMessageCoroutine;
    private Coroutine npcMessageCoroutine;
    private Coroutine npcNextMessageCoroutine;

    private RectTransform savedUserRect;
    private RectTransform savedNpcRect;
    private TMP_Text savedUserTimeTMP;
    private TMP_Text savedNPCMsgTMP;
    private TMP_Text savedNPCTimeTMP;
    private string originalTxt;

    public bool readOnly = false;

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
            userMessageCoroutine = StartCoroutine(UserMessageSpawnCoroutine(savedUserRect, savedUserTimeTMP));
        }

        if (npcMessageCoroutine != null)
        {
            npcMessageCoroutine = StartCoroutine(NPCMessageSpawnCoroutine(savedNpcRect, savedNPCMsgTMP, savedNPCTimeTMP, originalTxt));
        }

        if (npcNextMessageCoroutine != null)
        {
            npcNextMessageCoroutine = StartCoroutine(NPCNextMessageCoroutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetNewDialogue(DialogueBaseNodeSO newDialogue) => dialogueSO = newDialogue;
    public int GetIndex() => chatIndex;
    public void SetIndex(int index) { chatIndex = index; }

    public void Respond(int index)
    {
        Debug.Log("Respond index: " + index);

        CreateUserMessage(index);
        SetNewDialogue(dialogueSO.Choices[index].NextDialogue);
        DeleteChoices();

        if (dialogueSO == null) return;
        CreateNPCMessage();
        CreateChoiceButtons();


        if (dialogueSO.SkipChoices()) { SetNewDialogue(dialogueSO.NextNode); npcNextMessageCoroutine = StartCoroutine(NPCNextMessageCoroutine()); }
    }

    public void DeleteChoices()
    {
        choiceButtons.ForEach(button => { Destroy(button); });
        choiceButtons.Clear();
    }

    #region Create

    public void CreateNPCMessage()
    {
        // Instantiate NPC Message
        Debug.Log("Create NPC Message");
        GameObject npcMessageInstance = Instantiate(npcMessagePrefab, npcMessageContainer);

        npcMessageInstance.transform.localScale = Vector3.zero;

        // Set Time
        TMP_Text timeTMP = npcMessageInstance.transform.GetChild(1).GetComponent<TMP_Text>();
        savedNPCTimeTMP = timeTMP;

        // Set Dialogue Text
        TMP_Text dialogueTMP = npcMessageInstance.GetComponentInChildren<TMP_Text>();
        dialogueTMP.text = dialogueSO.Dialogue;
        savedNPCTimeTMP = dialogueTMP;

        // Animate Message
        StartCoroutine(NPCMessageSpawnCoroutine((RectTransform)npcMessageInstance.transform, dialogueTMP, timeTMP));

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

        // Instantiate Message
        GameObject userMessageInstance = Instantiate(userMessagePrefab, userMessageContainer);

        RectTransform userMsgRect = userMessageInstance.GetComponent<RectTransform>();

        TMP_Text userTimeTMP = userMsgRect.GetChild(1).GetComponent<TMP_Text>();
        userTimeTMP.text = TimeManager.Instance.GetTime();
        savedUserTimeTMP = userTimeTMP;

        // Set Text
        TMP_Text userTMP = userMessageInstance.GetComponentInChildren<TMP_Text>();
        userTMP.text = dialogueSO.GetChoiceText(choice);

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

        if (dialogueSO.SkipChoices()) { SetNewDialogue(dialogueSO.NextNode); StartCoroutine(NPCNextMessageCoroutine()); Debug.Log("Start next msg coroutine"); }
        else { CreateChoiceButtons(); npcNextMessageCoroutine = null; }
    }

    private IEnumerator UserMessageSpawnCoroutine(RectTransform t, TMP_Text timeTMP)
    {
        savedUserRect = t;

        t.DOScaleX(0f, .01f);
        t.DOScaleX(1f, .25f);

        timeTMP.text = TimeManager.Instance.GetTime();

        yield return new WaitForSeconds(.25f);

        userMessageCoroutine = null;
        yield return null;
    }

    private IEnumerator NPCMessageSpawnCoroutine(RectTransform t, TMP_Text msgTMP, TMP_Text timeTMP, string txt = "")
    {
        savedNpcRect = t;
        if (txt == "") originalTxt = msgTMP.text;
        else originalTxt = txt;

        DialogueManager.Instance.SetCanAnswer(false);
        DialogueManager.Instance.SetChatStatus(this.GetIndex() - 1, "typing...");

        while (!DialogueManager.Instance.GetCanAnswer())
        {
            timeTMP.text = "";
            t.DOScale(new Vector3(0f, 1f, 1f), .25f);
            yield return new WaitForSeconds(.5f);
            msgTMP.text = "";
            t.DOScaleX(.5f, .25f);
            msgTMP.text = ". . . ";
            yield return new WaitForSeconds(.5f);
            msgTMP.text = ". ";
            yield return new WaitForSeconds(.12f);
            msgTMP.text = " . . ";
            yield return new WaitForSeconds(.14f);
            msgTMP.text = ". . . ";
            yield return new WaitForSeconds(.1f);
            msgTMP.text = ". ";
            yield return new WaitForSeconds(.1f);
            msgTMP.text = " . . ";
            yield return new WaitForSeconds(.11f);
            msgTMP.text = ". . . ";
            yield return new WaitForSeconds(.17f);

            timeTMP.text = TimeManager.Instance.GetTime();
            DialogueManager.Instance.SetCanAnswer(true);
        }

        t.DOScale(new Vector3(1f,1f,1f), .5f);
        msgTMP.text = originalTxt;

        DialogueManager.Instance.SetNotification(this.GetIndex() - 1);
        DialogueManager.Instance.SetChatStatus(this.GetIndex() - 1, "online");
        
        if (!dialogueSO.SkipChoices()) ActivateButtons();
        else
        {
            Debug.Log("next coroutine???");
        }

        yield return new WaitForSeconds(1f);
        npcMessageCoroutine = null;
    }

    private void ActivateButtons()
    {
        choiceButtons.ForEach(button => { button.SetActive(true); });
    }


    #endregion



}
