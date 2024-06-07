using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class BlueScreen : MonoBehaviour
{

    [SerializeField] private TMP_Text _tmp;
    [SerializeField] private GameObject _ending;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(BlueScreenAnimation());
    }

    private IEnumerator BlueScreenAnimation()
    {

        _ending.transform.localRotation = new Quaternion(-20f, 0f, 0f, 0f);
        _ending.transform.localScale = Vector3.zero;

        float goal = 100f;
        float currentPercentage = 0f;

        while (currentPercentage < goal)
        {
            yield return new WaitForSeconds(.05f);
            currentPercentage += 1f;
            _tmp.text = currentPercentage.ToString() + "% complete.";
        }

        _ending.transform.DORotate(Vector3.zero, .5f);
        _ending.transform.DOScale(Vector3.one, .5f);
        _ending.SetActive(true);

        GameManager.Instance.TriggerEnd();
    }
} 
