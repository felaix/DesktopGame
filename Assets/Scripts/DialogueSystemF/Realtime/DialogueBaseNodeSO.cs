using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBaseNode", menuName = "Dialogue System")]
public class DialogueBaseNodeSO : ScriptableObject
{
    public string Dialogue;
    public NPC npc;

    [Header("Multiple Choices")]
    public List<Choice> Choices;

    [Header("Single Node")]
    public DialogueBaseNodeSO NextNode;
    public string GetChoiceText(int index) => Choices[index].ChoiceText;
    public bool SkipChoices() => NextNode != null;
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


[Serializable]
public enum NPC
{
    Mum,
    David,
    Hacker
}
