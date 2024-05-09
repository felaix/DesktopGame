using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDS
{
    [DefaultExecutionOrder(-1)]
    public class SpawnManager : MonoBehaviour
    {

        public static SpawnManager Instance;

        [SerializeField] private Transform enemyContainer;
        [SerializeField] private GameObject enemyPrefab;
        private int level = 0;

        private List<GameObject> enemies = new();

        public Action WaveCompleted = () => Debug.Log("wave completed");

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            enemies = new List<GameObject>();
        }

        private void FixedUpdate()
        {

            Debug.Log("enemy count: " + enemies.Count);
            if (enemies.Count == 0) WaveCompleted();

            //if (enemies != null && enemies.Count > 0 && enemies[0] == null)
            //{
            //    WaveCompleted();
            //    enemies.Clear();
            //}
        }

        public void SpawnWave()
        {
            level++;
            for (int i = 0; i < level; i++)
            {
                GameObject enemy = SpawnEnemy();
                enemies.Add(enemy);
            }
        }


        public GameObject SpawnEnemy() => Instantiate(enemyPrefab, enemyContainer);

        public int GetLevel() => level;

    }

}
