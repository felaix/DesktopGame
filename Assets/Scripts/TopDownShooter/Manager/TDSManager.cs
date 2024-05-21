using TDS;
using UnityEngine;

namespace TDS
{
    public class TDSManager : MonoBehaviour
    {
        public static TDSManager Instance;
        public int Wave;
        public int Coins;

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(this);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void ResetStats()
        {
            Wave = 0;
            Coins = 0;
        }

        private void OnDisable()
        {
            ResetStats();
        }
    }

}
