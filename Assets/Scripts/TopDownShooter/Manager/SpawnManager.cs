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

        [ReadOnly] public List<GameObject> enemies = new();
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

           WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            WaveCompleted -= OnWaveCompleted;
        }

        void Start()
        {
            enemies = new List<GameObject>();

            if (PlayOnLoad) StartGame();
        }

        private void FixedUpdate()
        {
            if (HasStarted && enemies.Count == 0) { HasStarted = false; WaveCompleted(); }
            else
            {

            }
        }

        private void OnEnable()
        {
            //WaveCompleted += OnWaveCompleted;
            if (!PlayOnEnable) return;

            Level = 0;
            CanSpawn = true;
            StartGame();
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

        public void ReloadGame()
        {
            CanSpawn = true;
            Level = 0;
            CanvasManager.Instance.ResetTimer();
            TDSManager.Instance.Coins = 0;
            TDSManager.Instance.Wave = 0;
            StartGame();
        }

        public void StartGame()
        {
            if (!CanSpawn) return;
            Level++;
            Debug.Log("Start game. Level: " + Level);
            HasStarted = true;
            SpawnWave();
        }

        public void SpawnWave()
        {
            TDSManager.Instance.Wave = Level;

            for (int i = 0; i < Level; i++)
            {
                if (!CanSpawn) return;
                Debug.Log("Level " + i + ".");
                GameObject enemy = SpawnEnemy();
                enemies.Add(enemy);
            }
        }

        private async void OnWaveCompleted()
        {

            if (Auto)
            {
                Debug.Log("Next Level. Awaiting Delay");
                await Task.Delay(1000);
                Debug.Log("Delay Done. Starting next lvl");
                StartGame();
            }
            else
            {
                Debugger.Instance.CreateLog("Auto not enabled");
            }
        }

        public void OnEnemyDeath(GameObject obj)
        {
            enemies.Remove(obj);
            if (enemies.Count <= 0) WaveCompleted();
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
