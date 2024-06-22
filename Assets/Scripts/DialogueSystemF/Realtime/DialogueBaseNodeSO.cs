using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueBaseNode", menuName = "Dialogue Node")]
public class DialogueBaseNodeSO : ScriptableObject
{

    [Header("NPC")]
    public NPCData NPCData;
    //public NPCNames NPCData.Name;

    [Header("Dialog")]
    public string Dialogue;
    public OnStartEvent OnStartAction;
    public OnClickEvent OnClickAction;
    //public OnClickEvent OnClickAction;
    public Sprite PhotoToSend;

    public string SFXSound;
    public string MusicSound;

    public float Delay = 1f;

    [Header("Multiple Choices")]
    public List<Choice> Choices;

    [Header("Single Node")]
    public DialogueBaseNodeSO NextNode;

    [Header("Trigger Node")]
    public DialogueBaseNodeSO TriggerDialogue;

    public Sprite GetProfilePicture() => NPCData.ProfilePicture;
    public string GetChoiceText(int index) => Choices[index].ChoiceText;
    public TraitType GetTrait(int index) => Choices[index].Trait;
    public bool SkipChoices() => NextNode != null;
    public DialogueBaseNodeSO GetChoiceNode(int index) => Choices[index].NextDialogue;
    public string GetTriggerNPCName() => TriggerDialogue.NPCData.Name.ToString();
    public string GetNPCName() => NPCData.Name.ToString();
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
    public TraitType Trait;
    public DialogueBaseNodeSO NextDialogue;
}

[Serializable]
public struct NPCData
{
    public Sprite ProfilePicture;
    public NPCNames Name;
}

[Serializable]
public enum OnClickEvent
{
    Null,
    ScaleImage,
    DownloadVirus,
    PrintHelloWorld
}


[Serializable]
public enum OnStartEvent
{
    Null,
    CallFromUnknown
}

[Serializable]
public enum NPCNames
{
    Mum,
    Dad,
    David,
    Hacker,
    Leon,
    Unknown,
    Nancy
}
