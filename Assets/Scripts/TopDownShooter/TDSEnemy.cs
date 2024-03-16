using UnityEngine;

namespace TDS.Elements
{
    using Variables;

    public class TDSEnemy 
    {

        private TDSStats stats;

        public void Initialize(TDSStats stats)
        {
            AddStats(stats);

            Debug.Log($"Enemy spawned. Attack: {this.stats.Attack}, HP: {this.stats.Health}, SPD: {this.stats.Speed}.");
        }

        private void AddStats(TDSStats tdsStats)
        {
            stats = tdsStats;
        }

        public TDSStats GetStats() => stats;

        private void Update() 
        {
            Move();
        }

        private void Move()
        {
            Vector3 targetPosition = GetPlayerPosition();
        }

        private Vector3 GetPlayerPosition()
        {
            
        }
    }

}
