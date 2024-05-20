using EditorAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TDS
{
    [DefaultExecutionOrder(-1)]
    public class SpawnManager : MonoBehaviour
    {

        public static SpawnManager Instance;

        [SerializeField] private List<Transform> enemySpawnPoints;
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject enemyPrefab;
        public int Level = 1;

        [ReadOnly] public List<GameObject> enemies = new();

        public Action WaveCompleted = () => Debug.Log("Wave Completed");
        public bool HasStarted = false;
        [ReadOnly] public bool CanSpawn = true;

        public bool GameStarted = false;
        public bool GamePaused = false;
        public bool PlayOnLoad = false;
        public bool PlayOnEnable = false;
        public bool Auto = false;

        private void Awake()
        {
            Instance = this;

            WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            WaveCompleted -= OnWaveCompleted;
        }

        private async void OnWaveCompleted()
        {

            if (Auto)
            {
                await Task.Delay(4000);
                StartGame();
                Debugger.Instance.CreateLog("Starting next level.");
            }
            else
            {
                Debugger.Instance.CreateLog("Auto not enabled");
            }
        }

        public void OnEnemyDeath(GameObject obj)
        {
            enemies.Remove(obj);
        }

        void Start()
        {
            enemies = new List<GameObject>();

            if (PlayOnLoad) StartGame();
        }

        private void FixedUpdate()
        {
            if (HasStarted && enemies.Count == 0) { HasStarted = false; WaveCompleted(); }
        }

        private void OnEnable()
        {
            if (PlayOnEnable) StartGame();
        }

        public void StopGame()
        {
            CanSpawn = false;
            foreach (GameObject obj in enemies)
            {
                if (obj != null)
                {
                    if (obj.TryGetComponent<Enemy>(out Enemy e))
                    {
                        e.Death();
                    }
                }
            }
        }

        public void StartGame()
        {
            if (!CanSpawn) return;
            Level++;
            HasStarted = true;
            SpawnWave();
        }

        public void SpawnWave()
        {
            for (int i = 0; i < Level; i++)
            {
                GameObject enemy = SpawnEnemy();
                enemies.Add(enemy);
            }
        }

        public GameObject SpawnEnemy(Transform spawnPoint = null)
        {
            if (spawnPoint == null) spawnPoint = GetRandomSpawnPoint();
            return Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, enemyContainer.transform);
        }

        public Transform GetRandomSpawnPoint()
        {
            int r = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
            return enemySpawnPoints[r];
        }

        public int GetLevel() => Level;

    }

}
