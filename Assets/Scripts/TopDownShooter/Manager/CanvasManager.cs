using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TDS
{
    public class CanvasManager : MonoBehaviour
    {

        public static CanvasManager Instance { get; private set; }

        [SerializeField] private TMP_Text waveTMP;
        [SerializeField] private TMP_Text playerHP;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _game;

        private void Awake()
        {

            if (Instance == null) { Instance = this ; } else Destroy(this);

            SpawnManager.Instance.WaveCompleted += ToggleWaveCompletedUI;
        }

        public void UpdatePlayerHP(int hp)
        {
            playerHP.color = Color.clear;
            playerHP.DOBlendableColor(Color.white, .25f).SetEase(Ease.InOutBounce);
            playerHP.text = hp.ToString();


            if (hp < 0)
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
            tmp.DOBlendableColor(Color.green, delay);
            tmp.DOFade(0f, 0f);
            tmp.DOFade(1f, delay);
            await Task.Delay((int)delay * 1000);
        }

        private void EnableWaveTMP() => waveTMP.gameObject.SetActive(true);

        private void DisableWaveTMP() => waveTMP.gameObject.SetActive(false);

        private async void TMPAnimation()
        {
            await FadeInTMP(waveTMP, 2f);
            await FadeOutTMP(waveTMP, 2f);

            DisableWaveTMP();
        }

        public void ToggleWaveCompletedUI()
        {
            waveTMP.text = "Level " + SpawnManager.Instance.GetLevel() + " completed.";
            TMPAnimation();
        }

        


    }
}
