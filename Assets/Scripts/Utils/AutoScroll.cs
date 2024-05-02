using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;

    void Update()
    {
        if (scrollRect != null)
        {
            // Scroll downwards continuously
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollSpeed * Time.deltaTime);
        }
    }
}
