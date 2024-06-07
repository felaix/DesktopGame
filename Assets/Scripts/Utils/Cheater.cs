using EditorAttributes;
using UnityEngine;
using TDS;
using System;

public class Cheater : MonoBehaviour
{

    [Header("Achievements")]
    public AchievementSpawner achievementSpawner;
    [Button("Spawn Achievement")] public void SpawnAchievement() => achievementSpawner.SpawnAchievement();

    [Header("Chat / NPC")]
    public DialogueBaseNodeSO npcNode;
    [Button("Create new Chat")] public void CreateDialogue() => DialogueManager.Instance.CreateChat(npcNode, npcNode.NPCData.Name.ToString());

    [Header("TDS")]
    public Item item;

    [Button("Spawn Wave")] public void SpawnEnemy () => SpawnManager.Instance.StartCoroutine(SpawnManager.Instance.SpawnWaveCoroutine());
    [Button("Spawn Item")] public void SpawnItem() => SpawnManager.Instance.SpawnItem(item, SpawnManager.Instance.GetPlayerSpawnPoint().position);

    [Button("Play music")] public void PlayMusic() => SoundManager.Instance.PlayMusic("HidingHorror");
    [Button("Trigger ending")] public void TriggerEnding() => CanvasManager.Instance.TriggerBlueScreen();

}

[Serializable]
public class AchievementSpawner
{
    public string achievementName;
    public int achievementPoints;

    public void SpawnAchievement() 
    {
        Achievement achievement = new Achievement
        {
            Name = achievementName,
            Points = achievementPoints
        };
        GameManager.Instance.AddAchievement(achievement);
    }
}
