using DG.Tweening;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDS
{
    [DefaultExecutionOrder(1)]

    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }

        [Header("TMP")]
        [SerializeField] private TMP_Text _waveTMP;
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

        [Header("Items")]
        [SerializeField] private Transform _itemIconContainer;
        [SerializeField] private GameObject _itemIconPrefab;
        [SerializeField] private List<AP_Explore> _exploreItems;

        private List<Item> _items = new();
        private bool _explored = false;

        private Color defaultWaveTMPColor;


        private void Awake()
        {
            if (Instance == null) { Instance = this ; } else Destroy(this);

            SpawnManager.Instance.WaveCompleted += ToggleWaveCompletedUI;
        }
        
        private void Update()
        {
            UpdateTimer();
        }

        private void OnDisable()
        {
            ResetTimer();
            //SpawnManager.Instance.StopGame();
            _gameOverScreen.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ResetTimer();
            //SpawnManager.Instance.StopGame();
        }

        private void Start()
        {
            defaultWaveTMPColor = _waveTMP.color;
            _mainMenu.SetActive(true);
            _game.SetActive(false);
        }

        public void RemoveExploreArea(AP_Explore item)
        {
            _exploreItems.Remove(item);
        }

        public void ActivateExploreArea()
        {
            List<AP_Explore> copy = _exploreItems;
            if (copy.Count <= 0) return;

            copy[0].gameObject.SetActive(true);
            if (copy[0].AutoNext && copy[1] != null) copy[1].gameObject.SetActive(true);

            copy.Clear();

            //if (_exploreItems.Count == 0) return;
            //if (_exploreItems[0] != null && _exploreItems[0].AutoNext && _exploreItems[1] != null) _exploreItems[1].gameObject.SetActive(true);
            //if (_exploreItems[0] != null) _exploreItems[0].gameObject.SetActive(true);
        }

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
        }

        public void ResetUI()
        {
            ResetTimer();
            ResetItems();
        }

        public void OnTimerCompleted()
        {
            ActivateExploreArea();
            SpawnManager.Instance.StopGame();
        }

        public void UpdateCoinTMP()
        {
            _coinTMP.text = TDSManager.Instance.Coins.ToString();
        }

        public void UpdateItems(Item item)
        {
            if (item.itemType == ItemType.Coin) return;
            if (item.itemType == ItemType.Shoes)
            {
                CreateItemIcon(item);
            }
            if (item.itemType == ItemType.Heart)
            {
                CreateItemIcon(item);
            }
            if (item.itemType == ItemType.BulletUpgrade)
            {
                CreateItemIcon(item);
            }
        }

        public void CreateItemIcon(Item item)
        {
            if (Instantiate(_itemIconPrefab, _itemIconContainer).TryGetComponent<Image>(out Image img)) {
                img.sprite = item.Sprite;
            }
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
            else if (hp > maxHP) _playerHealthTMP.color = Color.yellow;
            else _playerHealthTMP.color = Color.white;

            _playerHealthTMP.text = hp.ToString();

            if (hp <= 0)
            {
                _gameOverScreen.SetActive(true);
                _game.SetActive(false);
            }

        }

        private async Task FadeOutTMP(TMP_Text tmp, float delay)
        {
            tmp.DOBlendableColor(Color.white, delay);
            tmp.DOFade(0f, delay);
            await Task.Delay((int)delay * 1000);

        }

        private async Task FadeInTMP(TMP_Text tmp, float delay)
        {
            EnableWaveTMP();
            tmp.transform.DOJump(tmp.transform.position, 1f, 2, .5f).SetEase(Ease.InOutBounce);
            tmp.DOBlendableColor(defaultWaveTMPColor, delay);
            tmp.DOFade(1f, delay);
            await Task.Delay((int)delay * 1000);
        }

        private void EnableWaveTMP() => _waveTMP.gameObject.SetActive(true);

        private void DisableWaveTMP() => _waveTMP.gameObject.SetActive(false);

        private async void TMPAnimation()
        {
            await FadeInTMP(_waveTMP, 2f);
            await FadeOutTMP(_waveTMP, 4f);

            if (_waveTMP != null) DisableWaveTMP();
        }

        public void ToggleWaveCompletedUI()
        {
            _waveTMP.text = "Wave " + SpawnManager.Instance.GetLevel() + " completed.";
            TMPAnimation();
        }

        


    }
}
