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
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private GameObject responsePrefab;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private Transform responseContainer;

        private List<GameObject> choiceButtons = new();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            CreateMessage(currentDialogue);
            CreateResponse(currentDialogue);
        }

        private void SetNewDialogue(DialogueBaseNodeSO newDialogue) => currentDialogue = newDialogue;

        private void CreateMessage(DialogueBaseNodeSO dialogue)
        {
            TMP_Text dialogueText = Instantiate(messagePrefab, messageContainer).GetComponentInChildren<TMP_Text>();
            dialogueText.text = dialogue.Dialogue;
        }

        private void CreateResponse(DialogueBaseNodeSO dialogue)
        {
            for (int i = 0; i < currentDialogue.Choices.Count; i++)
            {
                GameObject response = Instantiate(responsePrefab, responseContainer);
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
            SetNewDialogue(currentDialogue.Choices[index].NextDialogue);
            DeleteChoices();
            CreateMessage(currentDialogue);
            CreateResponse(currentDialogue);
        }


    }

}
