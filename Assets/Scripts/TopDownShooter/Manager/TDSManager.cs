using System.Collections.Generic;
using System.ComponentModel;
using TDS;
using UnityEngine;

namespace TDS
{
    [DefaultExecutionOrder(-10)]
    public class TDSManager : MonoBehaviour
    {
        public static TDSManager Instance;
        public int Wave;
        public int Coins;
        public List<Item> Items;

        private Player player;

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(this);
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            if (player == null) { player = GameObject.FindWithTag("Player").GetComponent<Player>(); }
            player.IncreaseCoins(amount);
        }

        public void RemoveCoins(int amount)
        {
            Coins -= amount;
            if (Coins <= 0)
            {
                Coins = 0;
            }
            player.IncreaseCoins(-amount);
        }

        public void ResetStats()
        {
            Wave = 1;
            Coins = 0;
            Items.Clear();
        }

        private void OnDisable()
        {
            ResetStats();
        }
    }

}
