using EditorAttributes;
using UnityEngine;
using TDS;

public class Cheater : MonoBehaviour
{
    public DialogueBaseNodeSO npcNode;
    public Item item;

    [Button("Create new Chat")] public void CreateDialogue() => DialogueManager.Instance.CreateChat(npcNode, npcNode.npc.ToString());
    [Button("Spawn Wave")] public void SpawnEnemy () => SpawnManager.Instance.SpawnWave();
    [Button("Spawn Item")] public void SpawnItem() => SpawnManager.Instance.SpawnItem(item, SpawnManager.Instance.GetPlayerSpawnPoint().position);

}
