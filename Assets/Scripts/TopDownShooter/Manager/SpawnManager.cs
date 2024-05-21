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

        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject enemyPrefab;
        public int Level = 1;

        [ReadOnly] public List<Enemy> enemies = new();
        [ReadOnly] public List<GameObject> items = new();
        //[ReadOnly] public List<GameObject items = new ();

        public Action WaveCompleted = () => Debug.Log("Wave Completed");
        public bool HasStarted = false;
        public bool CanSpawn = true;

        public bool GameStarted = false;
        public bool GamePaused = false;
        public bool PlayOnLoad = false;
        public bool PlayOnEnable = false;
        public bool Auto = false;

        private void Awake()
        {
           Instance = this;

           //WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            WaveCompleted -= OnWaveCompleted;
        }

        void Start()
        {
            enemies = new List<Enemy>();

            if (PlayOnLoad) StartGame();
        }

        private void Update()
        {
            if (HasStarted && enemies.Count == 0 && CanSpawn && Auto) { HasStarted = false; CanSpawn = true; WaveCompleted(); }
        }

        private void OnEnable()
        {
            HasStarted = false;
            CanSpawn = true;
            WaveCompleted += OnWaveCompleted;
            if (!PlayOnEnable) return;

            Level = 0;
            CanSpawn = true;
            StartGame();
        }

        public void AddItem(GameObject item)
        {
            items.Add(item);
        }

        public void StopGame()
        {
            CanSpawn = false;
            KillAllEnemies();
        }

        public void KillAllEnemies()
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.Death();
                }
            }
        }

        public void ReloadGame()
        {
            // Reset Level & Conditiions
            CanSpawn = true;
            HasStarted = false;
            Level = 0;
            enemies.Clear();
            
            // Timer UI
            CanvasManager.Instance.ResetTimer();
            
            // Reset Manager
            TDSManager.Instance.Coins = 0;
            TDSManager.Instance.Wave = Level;

            //  *Start Game again*
            StartGame();
        }

        public void StartGame()
        {
            if (!CanSpawn || HasStarted) return;
            Level += 1;
            Debug.Log("Start wave " + Level);
            HasStarted = true;
            SpawnWave();
        }

        public void SpawnWave()
        {
            TDSManager.Instance.Wave = Level;

            for (int i = 0; i < Level; i++)
            {
                //if (!CanSpawn) return;
                Debug.Log("Level " + i + ".");
                GameObject enemy = SpawnEnemy();
                enemies.Add(enemy.GetComponent<Enemy>());
            }
        }

        private void OnWaveCompleted()
        {

            if (Auto)
            {
                // Has Started & CanSpawn has to be true to spawn

                //await Task.Delay(1000);
                HasStarted = false;
                //CanSpawn = true;
                StartGame();
            }
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            enemies.Remove(enemy);
            if (enemies.Count <= 0) { WaveCompleted(); HasStarted = false; }
        }

        public GameObject SpawnEnemy(Transform spawnPoint = null)
        {
            if (spawnPoint == null) spawnPoint = GetRandomSpawnPoint();
            return Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, enemyContainer.transform);
        }

        public Transform GetRandomSpawnPoint()
        {
            int r = UnityEngine.Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[r];
        }

        public int GetLevel() => Level;

    }

}
