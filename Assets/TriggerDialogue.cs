using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public DialogueBaseNodeSO _dialogue;
    public bool _triggerOnEnable = true;
    private void OnEnable()
    {
        if (!_triggerOnEnable) return;

        DialogueManager.Instance.CreateChat(_dialogue, _dialogue.NPCData.Name.ToString());
    }
}
