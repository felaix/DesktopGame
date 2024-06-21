using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Call : MonoBehaviour
{

    [SerializeField, Range(1, 5)] private float _delay;
    [SerializeField] private Button _declineBtn;
    [SerializeField] private Button _acceptBtn;

    private void Awake()
    {
        _declineBtn.onClick.AddListener(() => Decline());
        _acceptBtn.onClick.AddListener(() => Accept());
    }

    private void Decline()
    {
        // Play Decline Sound
        // SoundManager.Instance.PlaySFX("Ring");

        gameObject.SetActive(false);
    }

    private void Accept()
    {
        // Play Accept Sound
        // SoundManager.Instance.PlaySFX("Ring");

        // Invoke ? 
    }

    private IEnumerator CallingCoroutine()
    {
        yield return null;

        yield return new WaitForSeconds(_delay);

        // Play Ringing Sound
        // SoundManager.Instance.PlaySFX("Ring");

    }
}
