using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirusLoader : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _tmp;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _errorMsgPrefab;


    private void Start()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutBounce);
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return null;
        float goal = _slider.maxValue;

        while (_slider.value < goal)
        {
            _slider.value += _speed;
            yield return new WaitForSeconds(_speed);
        }

        _tmp.text = "Download completed.";
        _slider.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        Instantiate(_errorMsgPrefab, transform.parent);
    }

}
