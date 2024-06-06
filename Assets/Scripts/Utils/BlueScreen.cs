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
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(BlueScreenAnimation());
    }

    private IEnumerator BlueScreenAnimation()
    {   
        float goal = 100f;
        float currentPercentage = 0f;

        while (currentPercentage < goal)
        {
            yield return new WaitForSeconds(.05f);
            currentPercentage += 1f;
            _tmp.text = currentPercentage.ToString() + "% complete.";
        }

        _ending.transform.DORotate(Vector3.zero, .5f);
        _ending.SetActive(true);
    }
} 
