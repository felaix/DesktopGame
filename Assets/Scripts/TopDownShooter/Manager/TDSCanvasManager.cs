using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDS
{
    [DefaultExecutionOrder(-1)]
    public class TDSCanvasManager : MonoBehaviour
    {
        public static TDSCanvasManager Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefabTMP;

        [Header("TMP")]
        [SerializeField] private TMP_Text _playerHealthTMP;
        [SerializeField] private TMP_Text _coinTMP;

        [Header("Game over")]
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _game;

        [Header("Timer")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private float _maxDuration;
        [SerializeField] private float _duration;

        [Header("UI")]
        [SerializeField] private GameObject _mainMenu;
        public Color PrimaryColor;

        [Header("Items")]
        [SerializeField] private Transform _itemIconContainer;
        [SerializeField] private GameObject _itemIconPrefab;
        //[SerializeField] private List<AP_Explore> _exploreItems;

        private List<Item> _items = new();
        private bool _explored = false;

        //private Color defaultWaveTMPColor;
        private Player _player;


        private void Awake()
        {
            if (Instance == null) { Instance = this ; } else Destroy(this);

            SpawnManager.Instance.WaveCompleted += CreateWaveCompletedTMP;
        }
        private void Start()
        {
            _mainMenu.SetActive(true);
            _game.SetActive(false);
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void OnDisable()
        {
            ResetTimer();
            _gameOverScreen.SetActive(false);
        }

        private void OnEnable()
        {
            ResetTimer();

            _player = SpawnManager.Instance.GetPlayer();
        }

        public void OnTimerCompleted()
        {
            SpawnManager.Instance.StopGame();
        }

        #region Explore Area

        //public void RemoveExploreArea(AP_Explore item)
        //{
        //    _exploreItems.Remove(item);
        //}

        //public void AddExploreArea(AP_Explore item)
        //{
        //    _exploreItems.Add(item);
        //}

        //public void ActivateExploreArea() 
        //{
        //    if (_exploreItems[0] != null) _exploreItems[0].gameObject.SetActive(true);
        //}

        #endregion

        #region Reset UI
        public void ResetItems()
        {
            foreach(Transform item in _itemIconContainer)
            {
                Destroy(item.gameObject);
            }
        }

        public void ResetTimer()
        {
            _timerSlider.maxValue = _maxDuration;
            _timerSlider.value = _maxDuration;
            _duration = _maxDuration;
            SpawnManager.Instance.CanSpawn = true;
        }

        public void ResetUI()
        {
            ResetTimer();
            ResetItems();
        }

        #endregion

        #region Update UI

        public void UpdateCoinTMP()
        {
            _coinTMP.text = TDSManager.Instance.Coins.ToString();
            _coinTMP.transform.DOScale(1f, .1f);
        }

        public void UpdateItems(Item item)
        {
            CreateItemIcon(item);
            UpdateCoinTMP();
        }

        public void UpdateTimer()
        {
            float timer = Time.deltaTime;

            if (_duration > 0)
            {
                _duration -= timer;
            }else OnTimerCompleted();

            _timerSlider.value = (_duration);
        }

        public void UpdatePlayerHP(int hp, int maxHP = 3)
        {

            _playerHealthTMP.color = Color.clear;
            _playerHealthTMP.transform.DOScale(Vector3.one, .25f).SetEase(Ease.InOutBounce);

            if (hp == 1) _playerHealthTMP.color = Color.red;
            else if (hp >= maxHP) _playerHealthTMP.color = PrimaryColor;
            else _playerHealthTMP.color = Color.white;

            _playerHealthTMP.text = hp.ToString() + " / " + maxHP.ToString();

            if (hp <= 0)
            {
                _gameOverScreen.SetActive(true);
                _game.SetActive(false);
            }

        }

        #endregion

        #region Create
        public void CreateItemIcon(Item item)
        {
            var itemIcon = Instantiate(_itemIconPrefab, _itemIconContainer);

            if (itemIcon.TryGetComponent<Image>(out Image img))
            {
                img.sprite = item.ItemData.Sprite;
                img.transform.DOMoveY(img.transform.position.x + 10f, 2f);
                img.DOFade(0f, 2f);
            }
        }

        public void CreatePlayerTMP(string txt)
        {
            if (_player == null)
            {
                _player = SpawnManager.Instance.GetPlayer();
                if (_player == null) return;
            }

            TMP_Text tmp = CreatePlayerTMP(_player.GetPlayerCanvas());
            tmp.text = txt;

            PlayerTMPAnimation(tmp);
        }
        public TMP_Text CreatePlayerTMP(Transform container) => Instantiate(_playerPrefabTMP, container).GetComponent<TMP_Text>();
        public void CreateWaveCompletedTMP() => CreatePlayerTMP("Wave " + SpawnManager.Instance.GetLevel() + " completed.");

        #endregion

        #region Tweens

        // ONLY FADE OUT
        private Tween FadeOutTMP(TMP_Text tmp, float delay)
        {
            float randomDirectionX = Random.Range(-.4f, .4f);
            Vector3 direction = new Vector3(randomDirectionX, 1f, 0f);
            tmp.DOBlendableColor(Color.white, delay);
            tmp.transform.DOLocalMove(direction, delay);
            return tmp.DOFade(0f, delay);
        }

        // ONLY JUMP IN
        private Tween FadeJumpInTMP(TMP_Text tmp, float delay)
        {
            tmp.color = Color.clear;
            tmp.transform.DOJump(tmp.transform.position, .7f, 1, delay).SetEase(Ease.InOutBounce);
            Tween tween = tmp.DOBlendableColor(PrimaryColor, delay);

            return tween;
        }

        // JUMP IN AND OUT
        public Tween PlayerTMPAnimation(TMP_Text tmp)
        {
            Tween tween = FadeJumpInTMP(tmp, .35f);
            return tween.OnComplete(() => FadeOutTMP(tmp, .5f));
        }


        #endregion
        
    }
}
