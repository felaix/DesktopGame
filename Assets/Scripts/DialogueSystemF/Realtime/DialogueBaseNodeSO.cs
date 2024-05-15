using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBaseNode", menuName = "Dialogue Node")]
public class DialogueBaseNodeSO : ScriptableObject
{
    public string Dialogue;
    public NPC npc;

    public float Delay = 1f;

    [Header("Multiple Choices")]
    public List<Choice> Choices;

    [Header("Single Node")]
    public DialogueBaseNodeSO NextNode;

    [Header("Trigger Node")]
    public Trigger trigger;
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
public class Trigger
{
    public DialogueBaseNodeSO Dialogue;
    public NPC Npc;
}

[Serializable]
public enum NPC
{
    Mum,
    Dad,
    David,
    Hacker,
}
