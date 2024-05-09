using TMPro;
using UnityEngine;

namespace TDS
{
    public class CanvasManager : MonoBehaviour
    {

        [SerializeField] private TMP_Text waveTMP;

        private void OnEnable()
        {
            SpawnManager.Instance.WaveCompleted += ToggleWaveUI;
        }

        private void OnDisable()
        {
            SpawnManager.Instance.WaveCompleted -= ToggleWaveUI;
        }

        public void ToggleWaveUI()
        {
            waveTMP.text = SpawnManager.Instance.GetLevel() + " Level completed";
            waveTMP.gameObject.SetActive(!waveTMP);
        }


    }
}
