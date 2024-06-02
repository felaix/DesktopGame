using DG.Tweening;
using EditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TDS
{
    [DefaultExecutionOrder(-2)]
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
        [SerializeField] private Transform _spawnPointContainer;
        [SerializeField] private Transform _playerSpawnPoint;
        
        private List<Transform> _spawnPoints = new();
        private List<AP_Explore> _exploreItems = new();

        [Header("Container")]
        [SerializeField] private Transform _gameContainer;

        private Transform _enemyContainer;
        private Transform _itemContainer;

        [Header("Delay")]
        [SerializeField] private int _delayBetweenWaves;


        private List<Transform> _initialSpawnPoints;

        private Player _player;

        public Player GetPlayer() => _player;

        public Action WaveCompleted = () => Debug.Log("Wave Completed");
        public Action GameStart = () => Debug.Log("Game started");
        
        private void Awake()
        {
           Instance = this;

            WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            WaveCompleted -= OnWaveCompleted;

            _enemies.Clear();
            _items.Clear();
            //_exploreItems.Clear();

            if (_itemContainer != null) Destroy(_itemContainer.gameObject);
            if (_enemyContainer != null) Destroy(_enemyContainer.gameObject);
        }

        void Start()
        {
            _enemies = new List<Enemy>();
            _initialSpawnPoints = _spawnPoints;

            if (CreatePlayer && _player == null)
            {
                CreateNewPlayer();
            }
        }

        private void Update()
        {
            //if (GameStarted && _enemies.Count == 0 && CanSpawn && AutoCompleteWave) { GameStarted = false; CanSpawn = true; WaveCompleted(); }
        }

        private void OnEnable()
        {
            CreateContainer();

            if (!PlayOnEnable) return;

            //GameStarted = false;
            //CanSpawn = true;
            //WaveCompleted += OnWaveCompleted;

            Level = 0;
            StartGame();
        }

        public void AddExploreItem(AP_Explore item)
        {
            _exploreItems.Add(item);
        }

        public void RemoveExploreItem(AP_Explore item)
        {
            _exploreItems.Remove(item);
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
            WaveCompleted();
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        public void StopGame()
        {
            CanSpawn = false;
            KillAllEnemies();
            EnableAllExploreAreas();
        }

        private void EnableAllExploreAreas()
        {
            for (int i = 0; i < _exploreItems.Count; i++)
            {
                if (_exploreItems[i] != null) _exploreItems[i].gameObject.SetActive(true);
            }
        }

        public void DisableAllExploreAreas()
        {
            foreach (AP_Explore item in _exploreItems)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void ReloadGame()
        {
            // Reset Level & Conditiions
            CanSpawn = true;
            GameStarted = false;
            Level = 0;
            _enemies.Clear();

            // Reload UI
            TDSCanvasManager.Instance.ResetUI();

            // Clear Items
            ClearItems();

            // Reset Stats
            TDSManager.Instance.ResetStats();

            // Create a new Player
            CreateNewPlayer();

            // Reset Spawn Points
            if (_spawnPoints == null)
            {
                Debug.LogError("Spawnpoints null!");
            }
            else
            {
                _spawnPoints.Clear();
                if (_exploreItems != null && _exploreItems.Count > 0)
                {
                    _spawnPoints = CreateSpawnPoints(5, _exploreItems[0].transform.position);
                }
                else
                {
                    Debug.LogError("ExploreItems is null or empty!");
                }
            }

            // *Reset Camera + Transition to the beginning*
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

            GameStarted = false;
        }

        public int GetRandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max+1);
        }

        public void SetNewSpawnPoints(List<Transform> spawnPoints)
        {
            // Destroy & clear old spawn points
            //_spawnPoints.ForEach(p => Destroy(p.gameObject));
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

        private Transform CreateSpawnPointArea(Vector3 pos)
        {
            GameObject spawnPointArea = new GameObject();
            spawnPointArea.name = "Spawn Point Area";
            spawnPointArea.transform.SetParent(_spawnPointContainer);
            spawnPointArea.transform.position = pos;
            return spawnPointArea.transform;
        }

        private List<Transform> CreateSpawnPoints(int length, Vector3 centerPos, float radius = 1f)
        {
            if (length <= 0)
            {
                Debug.LogError("Error: Length must be greater than 0.");
                return new List<Transform>();
            }

            Transform area = CreateSpawnPointArea(centerPos);
            if (area == null)
            {
                Debug.LogError("Error: Failed to create spawn point area.");
                return new List<Transform>();
            }

            List<Transform> spawnPoints = new List<Transform>();

            for (int i = 0; i < length; i++)
            {
                GameObject spawnPoint = new GameObject();
                Transform t = spawnPoint.transform;

                spawnPoint.name = $"SpawnPoint {i}";

                t.SetParent(area);

                float angle = i * Mathf.PI * 2 / length;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                t.position = new Vector2(centerPos.x + x, centerPos.y + y);

                spawnPoints.Add(t);
            }

            return spawnPoints;
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
            
            if (spawnPoint == null) return null;

            Enemy enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity, _enemyContainer).GetComponent<Enemy>();

            int luck = 0;

            if (_player != null) luck = _player.GetPlayerLuck();

            enemy.SetDropItem(GetItemByLuck(luck));

            return enemy;
        }

        public Transform GetRandomSpawnPoint()
        {
            // Returns a random spawn point

            if (_spawnPoints == null || _spawnPoints.Count == 0) return null;

            int maxRange = _spawnPoints.Count;

            int r = UnityEngine.Random.Range(0, maxRange++);

            if (_spawnPoints[r].transform == null) return null;

            return _spawnPoints[r].transform;
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
        public void CreateNewPlayer() { if (_player != null) { Destroy(_player.gameObject); } _player = Instantiate(_playerPrefab, _playerSpawnPoint).GetComponent<Player>(); }

    }

}
