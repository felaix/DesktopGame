using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

public class Cheater : MonoBehaviour
{

    public DialogueBaseNodeSO npcNode;

    [Button("Create new Chat")]
    public void CreateDialogue() => DialogueManager.Instance.CreateChat(npcNode, npcNode.npc.ToString());

}
