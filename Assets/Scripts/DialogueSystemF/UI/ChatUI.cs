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
    private TMP_Text savedTMP;
    private string originalTxt;

    private void Start()
    {
        CreateNPCMessage();
        CreateChoiceButtons();
    }
    private void OnEnable()
    {
        if (userMessageCoroutine != null)
        {
            userMessageCoroutine = StartCoroutine(UserMessageSpawnCoroutine(savedUserRect));
        }

        if (npcMessageCoroutine != null)
        {
            npcMessageCoroutine = StartCoroutine(NPCMessageSpawnCoroutine(savedNpcRect, savedTMP, originalTxt));
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
        GameObject npcMessageInstance = Instantiate(npcMessagePrefab, npcMessageContainer);

        // Set Dialogue Text
        TMP_Text dialogueTMP = npcMessageInstance.GetComponentInChildren<TMP_Text>();
        dialogueTMP.text = dialogueSO.Dialogue;
        savedTMP = dialogueTMP;

        // Animate Message
        npcMessageCoroutine = StartCoroutine(NPCMessageSpawnCoroutine((RectTransform)npcMessageInstance.transform, dialogueTMP));

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

        // Set Text
        TMP_Text userTMP = userMessageInstance.GetComponentInChildren<TMP_Text>();
        userTMP.text = dialogueSO.GetChoiceText(choice);

        // Ensure the layout updates after adding a new message
        //LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());

        // Animate Message
        userMessageCoroutine = StartCoroutine(UserMessageSpawnCoroutine(userMsgRect));
    }

    #endregion

    #region Coroutines

    private IEnumerator NPCNextMessageCoroutine()
    {
        yield return new WaitForSeconds(2f);
        CreateNPCMessage();
        if (dialogueSO.SkipChoices()) { SetNewDialogue(dialogueSO.NextNode); npcNextMessageCoroutine = StartCoroutine(NPCNextMessageCoroutine()); yield return null; }
        else { CreateChoiceButtons(); }
        yield return new WaitForSeconds(.1f);

        npcNextMessageCoroutine = null;
    }

    private IEnumerator UserMessageSpawnCoroutine(RectTransform t)
    {
        savedUserRect = t;

        t.DOScaleX(0f, .01f);
        t.DOScaleX(1f, .25f);
        yield return new WaitForSeconds(.25f);

        userMessageCoroutine = null;
        yield return null;
    }

    private IEnumerator NPCMessageSpawnCoroutine(RectTransform t, TMP_Text tmp, string txt = "")
    {
        savedNpcRect = t;
        if (txt == "") originalTxt = tmp.text;
        else originalTxt = txt;

        DialogueManager.Instance.SetCanAnswer(false);
        DialogueManager.Instance.SetChatStatus(this.GetIndex(), "typing...");

        while (!DialogueManager.Instance.GetCanAnswer())
        {

            t.DOScaleX(0f, 0f);
            yield return new WaitForSeconds(.5f);
            tmp.text = "";
            t.DOScaleX(.5f, .25f);
            tmp.text = ". . . ";
            yield return new WaitForSeconds(.5f);
            tmp.text = ". ";
            yield return new WaitForSeconds(.1f);
            tmp.text = " . . ";
            yield return new WaitForSeconds(.1f);
            tmp.text = ". . . ";
            yield return new WaitForSeconds(.1f);
            tmp.text = ". ";
            yield return new WaitForSeconds(.1f);
            tmp.text = " . . ";
            yield return new WaitForSeconds(.1f);
            tmp.text = ". . . ";
            yield return new WaitForSeconds(.1f);

            DialogueManager.Instance.SetCanAnswer(true);
        }

        t.DOScaleX(1f, .25f);
        tmp.text = originalTxt;
        if (!dialogueSO.SkipChoices()) ActivateButtons();
        yield return new WaitForSeconds(.1f);
        //Debug.Log(chatIndex);
        //DialogueManager.Instance.SetChatStatus(chatIndex-1, "online");

        npcMessageCoroutine = null;
    }

    private void ActivateButtons()
    {
        choiceButtons.ForEach(button => { button.SetActive(true); });
    }


    #endregion



}
