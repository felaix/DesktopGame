using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineData : MonoBehaviour
{

    public static MineData instance {  get; private set; }

    //[SerializeField] private Sprite bombSprite; // To set the mine sprite

    [SerializeField] private List<Mine> mineList; // list of all mines
    [SerializeField] private int spawnBombCount = 5; // amount of bombs

    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Transform mineContainer;
    [SerializeField] private float bombChanceMultiplier = 30f;

    [Header("UI - Score")]
    [SerializeField] private int highScore;
    [SerializeField] private TMP_Text highScoreTMP;
    [SerializeField] private int score;
    [SerializeField] private TMP_Text scoreTMP;

    [Header("UI - When Bomb Explodes")]
    [SerializeField] private TMP_Text showScoreTMP;
    [SerializeField] private GameObject showScoreAndTryAgain;
    

    private int maxSpawnCount = 100;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        SpawnMines();
    }

    public void ShowTryAgain()
    {
        // set highscore when high enough
        if (score > highScore) highScore = score;
        
        // show ui
        showScoreAndTryAgain.SetActive(true);

        // Set tmp
        showScoreTMP.SetText("Score: " + score.ToString());
        highScoreTMP.SetText("Highscore: " + highScore.ToString());

        // Reset score after setting tmp
        score = 0;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreTMP.SetText("Score: " + score.ToString());
    }

    private bool ByChance()
    {
        float chance = spawnBombCount;

        float random = Random.Range(0, chance + bombChanceMultiplier);
        if (chance > random)
        {
            return true;
        }
        else return false;
    }

    private void SetBomb(Mine mine)
    {
        mine.isBomb = true;
    }

    public void ReloadMines()
    {

        Debug.Log("Reloading mines");

        foreach (var mine in mineList)
        {
            mine.ResetMines();

            if (ByChance())
            {
                SetBomb(mine);
            }

            mine.InitializeMine();
        }
    }

    private void SpawnMines()
    {
        for (int i = 0; i < maxSpawnCount; i++ )
        {
            GameObject mineInstance = Instantiate(minePrefab, mineContainer);
            Mine mine = mineInstance.GetComponent<Mine>();
            mineList.Add(mine);
            mine.mineId = i;

            if (ByChance())
            {
                SetBomb(mine);
            }


            mine.InitializeMine();
        }

        Debug.Log("Total amount of mines: " + mineList.Count);
    }

    public Mine GetMine(int mineIndex) {
        if (mineIndex < 0 || mineIndex >= mineList.Count) return null;
        if (mineList[mineIndex] == null) return null;
        else return mineList[mineIndex];
    }


}
