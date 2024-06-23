using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Call : MonoBehaviour
{

    [SerializeField, Range(1, 5)] private float _delay;
    [SerializeField] private Button _declineBtn;
    [SerializeField] private Button _acceptBtn;
    [SerializeField] private TMP_Text _timeTMP;

    private bool _triggered;
    private float _time;

    private void Awake()
    {
        _declineBtn.onClick.AddListener(() => Decline());
        _acceptBtn.onClick.AddListener(() => Accept());
    }

    private void OnEnable()
    {
        _triggered = true;
        StartCoroutine(CallingCoroutine());
    }

    private void Decline()
    {
        // Play Decline Sound

        _triggered = false;

        gameObject.SetActive(false);
    }

    private void Accept()
    {
        // Play Accept Sound
        // SoundManager.Instance.PlaySFX("Ring");

        // Invoke ? 

        _triggered = false;

    }

    private IEnumerator CallingCoroutine()
    {
        yield return null;

        while (_triggered)
        {
            yield return new WaitForSeconds(_delay);

            // Play Ringing Sound
            SoundManager.Instance.PlaySFX("TelephoneRing");
        }

        _timeTMP.gameObject.SetActive(true);
        _declineBtn.transform.parent.gameObject.SetActive(false);
        StartCoroutine(Calling());

        while (!_triggered)
        {
            yield return new WaitForSeconds(1);
            _time += 1f;
            _timeTMP.text = _time.ToString() + ":00";

            if (_time == 12)
            { 
                gameObject.SetActive(false);
                yield return null;            
            }
        }
    }

    public void AcceptCall()
    {
        StartCoroutine(Calling());
    }

    private IEnumerator Calling()
    {
        yield return new WaitForSeconds(1);

        SoundManager.Instance.PlaySFX("UnknownVoice");
    }

}
