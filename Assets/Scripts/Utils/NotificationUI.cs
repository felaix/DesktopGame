using DG.Tweening;
using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{

    [SerializeField] private TMP_Text _npcTMP;

    private void Initialize(string npc)
    {
        _npcTMP.text = npc;
    }

    public void CreateNotification(string npc)
    {
        Initialize(npc);
        transform.localScale = new Vector3(0,1,1);
        Tween tween = transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBack);
        tween.OnComplete(() => transform.DOMoveX(30f, 2f).SetEase(Ease.InOutBack));
    }

}
