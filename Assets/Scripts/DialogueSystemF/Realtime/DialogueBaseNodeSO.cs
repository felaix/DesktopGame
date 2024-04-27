using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBaseNode", menuName = "Dialogue System")]
public class DialogueBaseNodeSO : ScriptableObject
{
    public string Dialogue;
    public List<Choice> Choices;
    public string GetChoiceText(int index) => Choices[index].ChoiceText;
    public DialogueBaseNodeSO GetChoiceNode(int index) => Choices[index].NextDialogue;
    public virtual void Intialize(string dialogue, List<Choice> choices)
    {
        Dialogue = dialogue;
        Choices = choices;
    }
}

[Serializable]
public class Choice
{
    public string ChoiceText;
    public DialogueBaseNodeSO NextDialogue;
}
