using UnityEngine;
using UnityEngine.UI;

public class ActionInvokerImage : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Setting IMAGE ");
        ActionInvoker.Instance.SetPhotoSprite(this.GetComponent<Image>().sprite);
    }
}
