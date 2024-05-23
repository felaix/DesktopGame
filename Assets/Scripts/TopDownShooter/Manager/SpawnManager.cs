using DG.Tweening;
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

        [Header("Informations"), HelpBox("Development flow informations read only")]
        [ReadOnly] public int Level = 1;
        [ReadOnly] public bool GameStarted = false;
        [ReadOnly] public bool CanSpawn = true;

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;

        [Header("Spawn Points & Container")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject enemyContainer;


        private List<Enemy> _enemies = new();
        private List<GameObject> _items = new();

        public Action WaveCompleted = () => Debug.Log("Wave Completed");


        public bool PlayOnEnable = false;
        public bool AutoCompleteWave = false;
        public bool CreatePlayer = true;

        public Player GetPlayer() => _player;
        private Player _player;

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
            _enemies = new List<Enemy>();

            if (CreatePlayer)
            {
                CreateNewPlayer();
            }
        }

        private void Update()
        {
            if (GameStarted && _enemies.Count == 0 && CanSpawn && AutoCompleteWave) { GameStarted = false; CanSpawn = true; WaveCompleted(); }
        }

        private void OnEnable()
        {
            GameStarted = false;
            CanSpawn = true;
            WaveCompleted += OnWaveCompleted;
            if (!PlayOnEnable) return;

            Level = 0;
            CanSpawn = true;
            StartGame();
        }

        public void AddItem(GameObject item)
        {
            _items.Add(item);
        }

        public void KillAllEnemies()
        {

            if (_enemies.Count == 0) return;

            List<Enemy> copy = _enemies;

            foreach (Enemy enemy in copy)
            {
                enemy.Death();
            }

            copy.Clear();
        }

        public void StopGame()
        {
            CanSpawn = false;
            KillAllEnemies();
        }

        public void ReloadGame()
        {
            // Reset Level & Conditiions
            CanSpawn = true;
            GameStarted = false;
            Level = 0;
            _enemies.Clear();

            // Reload UI
            CanvasManager.Instance.ResetUI();

            // Reset Manager
            TDSManager.Instance.ResetStats();
            //TDSManager.Instance.Coins = 0;
            //TDSManager.Instance.Wave = Level;

            CreateNewPlayer();

            // Reset Camera
            Vector3 camStartPos = new Vector3(-15f, 0f, -10f);
            Camera camera = Camera.main;
            camera.transform.DOMove(camStartPos, 1f);

            //  *Start Game again*
            StartGame();
        }

        public void StartGame()
        {
            if (!CanSpawn || GameStarted) return;

            // Increase wave level
            Level += 1;
            TDSManager.Instance.Wave = Level;

            // Set Started to true
            GameStarted = true;

            // Spawn wave
            SpawnWave();
        }

        public void SpawnWave()
        {
            for (int i = 0; i < Level; i++)
            {
                GameObject enemy = SpawnEnemy();
                _enemies.Add(enemy.GetComponent<Enemy>());
            }
        }

        public void SetNewSpawnPoints(List<Transform> spawnPoints)
        {
            // Destroy & clear old spawn points
            _spawnPoints.ForEach(p => Destroy(p.gameObject));
            _spawnPoints.Clear();

            // Assign new List of spawnPoints
            _spawnPoints = spawnPoints;

            // Allow Spawning
            CanSpawn = true;
        }

        private void OnWaveCompleted()
        {
            GameStarted = false;

            if (AutoCompleteWave) StartGame();
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            _enemies.Remove(enemy);
            if (_enemies.Count <= 0) { WaveCompleted(); GameStarted = false; }
        }

        public GameObject SpawnEnemy(Transform spawnPoint = null)
        {
            if (spawnPoint == null) spawnPoint = GetRandomSpawnPoint();
            return Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity, enemyContainer.transform);
        }

        public Transform GetRandomSpawnPoint()
        {
            int r = UnityEngine.Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[r];
        }

        public int GetLevel() => Level;
        public async void CreateNewPlayer() { if (_player != null) Destroy(_player.gameObject); await Task.Delay(100); _player = Instantiate(_playerPrefab, _playerSpawnPoint).GetComponent<Player>(); }
    }

}
