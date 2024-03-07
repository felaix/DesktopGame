using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int mineId;
    public bool isBomb;
    [SerializeField] private GameObject bomb;

    private int bombCount = 0;
    private TMP_Text displayText;

    private MineData mineData;
    private List<Mine> adjacentMines = new();

    private bool adjacentMinesFound = false;

    private void Start()
    {
        displayText.SetText(mineId.ToString());
    }

    private void Awake()
    {

        displayText = GetComponentInChildren<TMP_Text>();
        mineData = MineData.instance;
    }

    public void ResetMines()
    {
        isBomb = false;
        adjacentMines.Clear();
        bombCount = 0;
        displayText.SetText("");
        adjacentMinesFound = false;
        bomb.SetActive(false);
    }

    private void Explode()
    {
        bomb.SetActive(isBomb);
        if (isBomb) displayText.SetText("");
        mineData.ShowTryAgain();
    }

    public void InitializeMine()
    {

        if (isBomb)
        {
            displayText.SetText("");
            displayText.color = Color.red;
        }else
        {
            displayText.SetText("");
            displayText.color = Color.white;
        }

    }

    private void ShowAdjacentMines()
    {
        foreach (Mine m in adjacentMines)
        {

            if (m == null) continue;
            else if (!m.isBomb)
            {
                m.GetAdjacentMines();
                m.displayText.SetText(m.bombCount.ToString());
            }
        }
    }

    public void ClickOnMine()
    {
        GetAdjacentMines();
        if (isBomb) Explode();
        else ShowAdjacentMines();
        displayText.SetText(bombCount.ToString());
        mineData.AddScore(bombCount);
    }

    public void GetAdjacentMines()
    {

        // ! Gets called when clicked on mine

        if (!adjacentMinesFound)
        {
            FindAdjacentMines(); // Adds all mines to adjacentMineList

            foreach (Mine m in adjacentMines)
            {

                if (m == null) continue;

                if (m.isBomb)
                {
                    bombCount++;
                }
            }
        }

        InitializeMine();

        //mineData.AddScore(bombCount);
    }

    private void FindAdjacentMines()
    {

        //!  Gets the adjacent mines

        //! Pattern:
        // If it's a number like 10, 20, 30, 40 etc. it does skip for +9, -11 or -1
        // If it's a number like 9, 19, 29, 39 etc. it does skip for +1, +11 or -9

        if (!Mathf.Approximately(mineId % 10, 0)) { if (mineData.GetMine(mineId + 9) != null) adjacentMines.Add(mineData.GetMine(mineId + 9)); }

        if (mineId % 10 != 9) { if (mineData.GetMine(mineId + 1) != null) adjacentMines.Add(mineData.GetMine(mineId + 1)); }
        if (mineData.GetMine(mineId + 10) != null) adjacentMines.Add(mineData.GetMine(mineId + 10));
        if (mineId % 10 != 9) { if (mineData.GetMine(mineId + 11) != null) adjacentMines.Add(mineData.GetMine(mineId + 11)); }


        if (!Mathf.Approximately(mineId % 10, 0)) { if (mineData.GetMine(mineId - 11) != null) adjacentMines.Add(mineData.GetMine(mineId - 11)); }

        if (mineId % 10 != 9) if (mineData.GetMine(mineId - 9) != null) adjacentMines.Add(mineData.GetMine(mineId - 9));

        if (!Mathf.Approximately(mineId % 10, 0)) { if (mineData.GetMine(mineId - 1) != null) adjacentMines.Add(mineData.GetMine(mineId - 1)); }

        if (mineData.GetMine(mineId - 10) != null) adjacentMines.Add(mineData.GetMine(mineId - 10));

        adjacentMinesFound = true;

    }

}
