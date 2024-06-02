using System.Collections;
using TMPro;
using UnityEngine;

public class BlueScreen : MonoBehaviour
{

    [SerializeField] private TMP_Text _tmp;

    void Start()
    {
        StartCoroutine(BlueScreenAnimation());
    }

    private IEnumerator BlueScreenAnimation()
    {

        float goal = 100f;
        float currentPercentage = 0f;

        while (currentPercentage < goal)
        {
            yield return new WaitForSeconds(.1f);
            currentPercentage += 1f;
            _tmp.text = currentPercentage.ToString() + "% complete.";
        }

    }

}
