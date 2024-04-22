using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Color hoverColor;

    private Color defaultColor;
    [SerializeField] private Image icon;

    private void Start()
    {
        if (icon == null) icon = GetComponent<Image>();
        defaultColor = icon.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon == null) return;
        icon.DOColor(hoverColor, .25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (icon == null) return;
        icon.DOColor(defaultColor, .25f);
    }
}
