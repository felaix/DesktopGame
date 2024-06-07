using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static Debugger dbgr;

    [ReadOnly] public PlayerTraits currentPlayerTraits;

    [ReadOnly] public List<Achievement> PlayerAchievements;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
        if (!dbgr) dbgr = gameObject.AddComponent<Debugger>();

        DontDestroyOnLoad(gameObject);
    }

    public void AddAchievement(Achievement achievement)
    {
        PlayerAchievements.Add(achievement);
        CanvasManager.Instance.CreateAchievement(achievement.Name, achievement.Points);
    }

    public void CalculateAchievements()
    {
        if (currentPlayerTraits.Emotional == 2)
        {
            Achievement emotionalAchievement = new Achievement();
            emotionalAchievement.Name = "Be emotional";
            emotionalAchievement.Points = currentPlayerTraits.Emotional;
            AddAchievement(emotionalAchievement);
        }

        if (currentPlayerTraits.Reserved == 2)
        {
            Achievement reservedAchievement = new Achievement();
            reservedAchievement.Name = "Be reserved";
            reservedAchievement.Points = currentPlayerTraits.Reserved;
            AddAchievement(reservedAchievement);
        }

        if (currentPlayerTraits.Polite == 2)
        {
            Achievement politeAchievement = new Achievement();
            politeAchievement.Name = "Be polite";
            politeAchievement.Points = currentPlayerTraits.Polite;
            AddAchievement(politeAchievement);
        }
    }

    public void AddPlayerTrait(TraitType trait)
    {
        switch (trait)
        {
            case TraitType.Neutral:
                return;
            case TraitType.Emotional:
                currentPlayerTraits.Emotional++;
                break;
            case TraitType.Polite:
                currentPlayerTraits.Polite++;
                break;
            case TraitType.Reserved:
                currentPlayerTraits.Reserved++;
                break;
        }
    } 

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int lvl)
    {
        StartCoroutine(LoadSceneCoroutine(lvl, 1f));
    }
    
    private IEnumerator LoadSceneCoroutine(int lvl, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(lvl);
    }

}

[System.Serializable]
public class Achievement
{
    public string Name;
    public int Points;
}


[System.Serializable]
public class PlayerTraits
{
    public int Emotional;
    public int Polite;
    public int Reserved;
}

[System.Serializable]
public enum TraitType
{
    Neutral,
    Emotional,
    Polite,
    Reserved
}
