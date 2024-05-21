using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDS
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }

        [SerializeField] private TMP_Text waveTMP;
        [SerializeField] private TMP_Text playerHP;

        [Header("Game over")]
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _game;

        [Header("Timer")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private float _maxDuration;
        [SerializeField] private float _duration;

        [SerializeField] private TMP_Text coinTMP;

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
            defaultWaveTMPColor = waveTMP.color;
        }

        public void ResetTimer()
        {
            _timerSlider.maxValue = _maxDuration;
            _timerSlider.value = _maxDuration;
            _duration = _maxDuration;
        }

        public void OnTimerCompleted()
        {
            SpawnManager.Instance.StopGame();
        }

        public void UpdateCoinTMP()
        {
            coinTMP.text = TDSManager.Instance.Coins.ToString();
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

        public void UpdatePlayerHP(int hp)
        {
            playerHP.color = Color.clear;
            playerHP.DOBlendableColor(Color.white, .25f).SetEase(Ease.InOutBounce);
            playerHP.text = hp.ToString();


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

        private void EnableWaveTMP() => waveTMP.gameObject.SetActive(true);

        private void DisableWaveTMP() => waveTMP.gameObject.SetActive(false);

        private async void TMPAnimation()
        {
            await FadeInTMP(waveTMP, 2f);
            await FadeOutTMP(waveTMP, 4f);

            DisableWaveTMP();
        }

        public void ToggleWaveCompletedUI()
        {
            waveTMP.text = "Wave " + SpawnManager.Instance.GetLevel() + " completed.";
            TMPAnimation();
        }

        


    }
}
