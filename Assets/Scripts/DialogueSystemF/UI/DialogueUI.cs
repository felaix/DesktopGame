using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    private TMP_Text dialogueTMP;

    public void Start()
    {
        dialogueTMP = transform.GetComponentInChildren<TMP_Text>();
    }

    public void SetDialogue(DialogueBaseNodeSO node)
    {
        dialogueTMP.text = node.Dialogue;
    }

}
