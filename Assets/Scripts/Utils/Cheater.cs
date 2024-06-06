using EditorAttributes;
using UnityEngine;
using TDS;

public class Cheater : MonoBehaviour
{
    public DialogueBaseNodeSO npcNode;
    public Item item;

    private BlueScreen blueScreen;

    private void Awake()
    {
        blueScreen = FindObjectOfType<BlueScreen>();
    }

    private BlueScreen GetBlueScreen() => FindObjectOfType<BlueScreen>();

    [Button("Create new Chat")] public void CreateDialogue() => DialogueManager.Instance.CreateChat(npcNode, npcNode.NPCData.Name.ToString());
    [Button("Spawn Wave")] public void SpawnEnemy () => SpawnManager.Instance.StartCoroutine(SpawnManager.Instance.SpawnWaveCoroutine());
    [Button("Spawn Item")] public void SpawnItem() => SpawnManager.Instance.SpawnItem(item, SpawnManager.Instance.GetPlayerSpawnPoint().position);
    [Button("Play music")] public void PlayMusic() => SoundManager.Instance.PlayMusic("HidingHorror");
    [Button("Trigger ending")] public void TriggerEnding() => blueScreen.gameObject.SetActive(true);


}
