using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            CreateNPCMessage(currentDialogue);
            CreateResponse(currentDialogue);
        }

        private void SetNewDialogue(DialogueBaseNodeSO newDialogue) => currentDialogue = newDialogue;

        private void CreateNPCMessage(DialogueBaseNodeSO dialogue)
        {
            TMP_Text dialogueText = Instantiate(npcMessagePrefab, npcMessageContainer).GetComponentInChildren<TMP_Text>();
            dialogueText.text = dialogue.Dialogue;
        }

        private void CreateUserMessage(DialogueBaseNodeSO dialogue, int choice)
        {
            TMP_Text userTMP = Instantiate(userMessagePrefab, npcMessageContainer).GetComponentInChildren<TMP_Text>();
            userTMP.text = dialogue.GetChoiceText(choice);
        }

        private void CreateResponse(DialogueBaseNodeSO dialogue)
        {
            for (int i = 0; i < currentDialogue.Choices.Count; i++)
            {
                GameObject response = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                choiceButtons.Add(response);
                TMP_Text responseTMP = response.GetComponentInChildren<TMP_Text>();
                response.GetComponent<DialogueResponseButton>().index = i;
                responseTMP.text = dialogue.GetChoiceText(i);
            }
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
            CreateResponse(currentDialogue);
        }


    }

}
