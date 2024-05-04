using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace DS
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private DialogueBaseNodeSO currentDialogue;

        [Header("Chat UI ")]
        [SerializeField] private GameObject npcMessagePrefab;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private GameObject userMessagePrefab;

        [SerializeField] private Transform npcMessageContainer;
        [SerializeField] private Transform choiceButtonContainer;
        [SerializeField] private Transform userMessageContainer;

        private List<GameObject> choiceButtons = new();

        private bool canAnswer = false;

        public bool CanAnswer() => canAnswer;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            CreateNPCMessage(currentDialogue);
            CreateChoiceButtons(currentDialogue);
        }

        private void SetNewDialogue(DialogueBaseNodeSO newDialogue) => currentDialogue = newDialogue;

        private void CreateNPCMessage(DialogueBaseNodeSO dialogue)
        {
            // Instantiate NPC Message
            GameObject npcMessageInstance = Instantiate(npcMessagePrefab, npcMessageContainer);

            // Set Dialogue Text
            TMP_Text dialogueTMP = npcMessageInstance.GetComponentInChildren<TMP_Text>();
            dialogueTMP.text = dialogue.Dialogue;

            // Animate Message
            StartCoroutine(NPCMessageSpawnCoroutine((RectTransform)npcMessageInstance.transform, dialogueTMP));

        }

        private void CreateUserMessage(DialogueBaseNodeSO dialogue, int choice)
        {
            if (dialogue.SkipChoices()) return;

            // Instantiate Message
            GameObject userMessageInstance = Instantiate(userMessagePrefab, npcMessageContainer);

            RectTransform userMsgRect = userMessageInstance.GetComponent<RectTransform>();

            // Set Text
            TMP_Text userTMP = userMessageInstance.GetComponentInChildren<TMP_Text>();
            userTMP.text = dialogue.GetChoiceText(choice);

            // Ensure the layout updates after adding a new message
            LayoutRebuilder.ForceRebuildLayoutImmediate(npcMessageContainer.GetComponent<RectTransform>());

            // Animate Message
            StartCoroutine(UserMessageSpawnCoroutine(userMsgRect));
        }

        private IEnumerator UserMessageSpawnCoroutine(RectTransform t)
        {

            t.DOScaleX(0f, .01f);
            t.DOScaleX(1f, .25f);

            yield return null;
        }

        private void CreateChoiceButtons(DialogueBaseNodeSO dialogue)
        {
            if (dialogue.SkipChoices()) return;

            for (int i = 0; i < currentDialogue.Choices.Count; i++)
            {
                GameObject choiceInstance = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                choiceButtons.Add(choiceInstance);
                TMP_Text responseTMP = choiceInstance.GetComponentInChildren<TMP_Text>();
                choiceInstance.GetComponent<DialogueResponseButton>().index = i;
                responseTMP.text = dialogue.GetChoiceText(i);
                choiceInstance.SetActive(false);
            }
        }

        private void ActivateButtons()
        {
            choiceButtons.ForEach(button => { button.SetActive(true); });
        }

        public void DeleteChoices()
        {
            choiceButtons.ForEach(button => { Destroy(button); });
            choiceButtons.Clear();
        }

        public void OnRespond(int index)
        {
            CreateUserMessage(currentDialogue, index);

            SetNewDialogue(currentDialogue.Choices[index].NextDialogue);
            DeleteChoices();

            CreateNPCMessage(currentDialogue);
            CreateChoiceButtons(currentDialogue);

            if (currentDialogue.SkipChoices()) { SetNewDialogue(currentDialogue.NextNode); StartCoroutine(NPCNextMessageCoroutine()); }
        }

        #region Coroutines

        private IEnumerator NPCNextMessageCoroutine()
        {
            yield return new WaitForSeconds(2f);
            CreateNPCMessage(currentDialogue);
            CreateChoiceButtons(currentDialogue);
        }

        private IEnumerator HideChoicesCoroutine(RectTransform[] rectTransforms)
        {
            yield return null;
        }

        private IEnumerator NPCMessageSpawnCoroutine(RectTransform t, TMP_Text tmp)
        {
            string origText = tmp.text;

            canAnswer = false;

            while (!canAnswer)
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

                canAnswer = true;
            }

            t.DOScaleX(1f, .25f);
            tmp.text = origText;
            if (!currentDialogue.SkipChoices()) ActivateButtons();

            yield return null;
        }

        #endregion



    }

}
