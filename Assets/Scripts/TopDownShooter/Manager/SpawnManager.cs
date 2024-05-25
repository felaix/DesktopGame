using DG.Tweening;
using EditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TDS
{
    [DefaultExecutionOrder(-1)]
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;

        [Header("Development Conditions"), HelpBox("Development flow informations")]
        [ReadOnly] public int Level = 1;
        [ReadOnly] public bool GameStarted = false;
        [ReadOnly] public bool CanSpawn = true;
        public bool PlayOnEnable = false;
        [ReadOnly] public bool AutoCompleteWave = true;
        [ReadOnly] public bool CreatePlayer = true;

        private List<Enemy> _enemies = new();
        private List<Item> _items = new();

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;

        [Header("Items")]
        [SerializeField] private List<Item> _itemsToDrop;
        [SerializeField] private List<Item> _luckyItemsToDrop;

        [Header("Spawn Points")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private Transform _playerSpawnPoint;

        [Header("Container")]
        [SerializeField] private Transform _gameContainer;
        private Transform _enemyContainer;
        private Transform _itemContainer;

        [Header("Delay")]
        [SerializeField] private int _delayBetweenWaves;

        private Player _player;

        public Player GetPlayer() => _player;

        public Action WaveCompleted = () => Debug.Log("Wave Completed");
        public Action GameStart = () => Debug.Log("Game started");
        
        private void Awake()
        {
           Instance = this;

            if (CreatePlayer && _player == null)
            {
                CreateNewPlayer();
            }

            WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            WaveCompleted -= OnWaveCompleted;

            _enemies.Clear();
            _items.Clear();

            Destroy(_itemContainer);
            Destroy(_enemyContainer);
        }

        void Start()
        {
            _enemies = new List<Enemy>();
        }

        private void Update()
        {
            //if (GameStarted && _enemies.Count == 0 && CanSpawn && AutoCompleteWave) { GameStarted = false; CanSpawn = true; WaveCompleted(); }
        }

        private void OnEnable()
        {
            CreateContainer();

            if (!PlayOnEnable) return;

            GameStarted = false;
            CanSpawn = true;
            //WaveCompleted += OnWaveCompleted;

            Level = 0;
            StartGame();
        }

        private void CreateContainer()
        {
            if (_itemContainer == null) { _itemContainer = Instantiate(new GameObject(), _gameContainer).GetComponent<Transform>(); _itemContainer.name = "Item Container"; }
            if (_enemyContainer == null) { _enemyContainer = Instantiate(new GameObject(), _gameContainer).GetComponent<Transform>(); _enemyContainer.name = "Enemy Container"; }
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public void KillAllEnemies()
        {
            if (_enemies.Count == 0) return;

            List<Enemy> copy = _enemies;

            for (int i = 0; i < copy.Count; i++)
            {
                _enemies[i].Death();
            }

            _enemies.Clear();
        }

        public void DestroyAllItems()
        {
            if (_items.Count == 0) return;

            List<Item> copy = _items;

            foreach(Item item in copy)
            {
                Destroy(item.gameObject);
            }

            _items.Clear();
        }

        public void StopGame()
        {
            CanSpawn = false;
            KillAllEnemies();
            //DestroyAllItems();
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

            // Destroy all Items
            DestroyAllItems();

            // Reset Stats
            TDSManager.Instance.ResetStats();

            // Create a new Player
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

            if (Level < 5)
            {
                Level += 1;
                TDSManager.Instance.Wave = Level;
            }


            // Set Started to true
            GameStarted = true;

            // Spawn wave
            StartCoroutine(SpawnWaveCoroutine());
        }

        public IEnumerator SpawnWaveCoroutine()
        {
            yield return new WaitForSeconds(_delayBetweenWaves);

                for (int i = 0; i < Level; i++)
                {
                    if (!CanSpawn) continue;
                    Enemy enemy = SpawnEnemy();
                    _enemies.Add(enemy);
                }
        }

        public int GetRandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max+1);
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

            if (CanSpawn) StartGame();
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            _enemies.Remove(enemy);
            if (_enemies.Count <= 0) { WaveCompleted(); }
        }

        public Transform GetPlayerSpawnPoint() => _playerSpawnPoint;

        public Transform SpawnItem(Item item, Vector3 pos = new())
        {
            if (pos == null) pos = _itemContainer.position;
            if (item == null) return null;
            else return Instantiate(item, pos, Quaternion.identity, _itemContainer).transform;
        }

        public Enemy SpawnEnemy(Transform spawnPoint = null, Item itemToDrop = null)
        {
            if (spawnPoint == null) spawnPoint = GetRandomSpawnPoint();
            
            Enemy enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity, _enemyContainer).GetComponent<Enemy>();

            int luck = 0;

            if (_player != null) luck = _player.GetPlayerLuck();

            enemy.SetDropItem(GetItemByLuck(luck));

            return enemy;
        }

        public Transform GetRandomSpawnPoint()
        {
            int r = UnityEngine.Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[r];
        }

        public Item GetItemByLuck(int luck)
        {
            if (luck < 1)
            {
                int NoItem = UnityEngine.Random.Range(0, 2);
                if (NoItem == 0) return null;
                return GetRandomItem();
            }
            else return GetRandomLuckyItem();
        }
        public Item GetRandomItem() => _itemsToDrop[UnityEngine.Random.Range(0, _itemsToDrop.Count)];
        public Item GetRandomLuckyItem() => _luckyItemsToDrop[UnityEngine.Random.Range(0, _itemsToDrop.Count)];

        public int GetLevel() => Level;
        public async void CreateNewPlayer() { if (_player != null) Destroy(_player.gameObject); await Task.Delay(100); _player = Instantiate(_playerPrefab, _playerSpawnPoint).GetComponent<Player>(); }

    }

}
