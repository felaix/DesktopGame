using UnityEngine;

namespace TDS.Elements
{
    using TDS.Variables;
    public class TDSElement : MonoBehaviour
    {

        [SerializeField] private static TDSPlayer Player;

        public static void Initialize()
        {
            Player = CreatePlayer();
            //AddEnemy(CreateEnemy());
        }

        private static TDSEnemy CreateEnemy()
        {
            GameObject newEnemy = new GameObject();
            newEnemy.AddComponent<TDSEnemy>();
            TDSEnemy instantiatedEnemy = GameObject.Instantiate(newEnemy, TDSController.instance.transform).GetComponent<TDSEnemy>();
            instantiatedEnemy.Initialize(CreateStats());
            instantiatedEnemy.name = "ENEMY SPAWNED HERE";
            Debug.Log("Spawn enemy. name:  " + instantiatedEnemy.name);
            return instantiatedEnemy;
        }

        private static TDSStats CreateStats(float hp = 10, float spd = 10, float atk = 10)
        {
            TDSStats stats = new TDSStats();
            stats.Attack = atk;
            stats.Speed = spd;
            stats.Health = hp;
            return stats;
        }

        private static TDSPlayer CreatePlayer()
        {
            GameObject playerGameObject = new GameObject();
            TDSPlayer player = playerGameObject.AddComponent<TDSPlayer>();
            TDSPlayer instantiatedPlayer = GameObject.Instantiate(playerGameObject).GetComponent<TDSPlayer>();
            instantiatedPlayer.Initialize(CreateStats());
            return instantiatedPlayer;
        }

    }

}
