using EditorAttributes;
using UnityEngine;
using TDS;

public class Cheater : MonoBehaviour
{
    public SpawnManager spawnManager;
    public DialogueBaseNodeSO npcNode;

    [Button("Create new Chat")]
    public void CreateDialogue() => DialogueManager.Instance.CreateChat(npcNode, npcNode.npc.ToString());

    [Button("Spawn Wave")]
    public void SpawnEnemy () => spawnManager.SpawnWave();

}
