using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AutoScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;

    private float spd;

    private void Start()
    {
        spd = scrollSpeed;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        scrollSpeed = 0f;
        //Debugger.Instance.CreateLog("Pointer Down!");

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scrollSpeed = spd;
        //Debugger.Instance.CreateLog("Pointer UP!");
    }

    void Update()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollSpeed * Time.deltaTime);
        }
    }
}
