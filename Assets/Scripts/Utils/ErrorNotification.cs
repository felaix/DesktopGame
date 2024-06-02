using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorNotification : MonoBehaviour
{
    [SerializeField] private Button _closeBtn;

    private void Start()
    {
        _closeBtn.onClick.AddListener(OnCloseBtnClick);

        transform.localScale = Vector3.zero;

        StartCoroutine(FadeInCoroutine());
    }

    private void OnCloseBtnClick()
    {
        transform.DOScale(Vector3.zero, .35f).SetEase(Ease.OutElastic);

        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i <= CanvasManager.Instance.ErrorNotificationCounter; i++)
        {
            GameObject errorNotif = CreateNewErrorButton();
            list.Add(errorNotif);
        }

        CanvasManager.Instance.IncreaseErrorNotificationCounter(list);

        list.Clear();

        SoundManager.Instance.PlaySFX("ButtonHover");
    }

    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-200f, 200f);
        float randomY = Random.Range(-100f, 100f);
        return new Vector2(randomX, randomY);
    }
    private GameObject CreateNewErrorButton() => Instantiate(this.gameObject, transform.parent);
    private IEnumerator FadeInCoroutine()
    {
        yield return new WaitForSeconds(.1f);

        transform.DOScale(Vector3.one, .35f).SetEase(Ease.InOutBounce);
        transform.localPosition = GetRandomPosition();

    }

}
