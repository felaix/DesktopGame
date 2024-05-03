using DS;
using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseButton : MonoBehaviour
{
    public int index;

    private Button btn;
    private Transform container;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Respond);

        container = GetComponentInParent<Transform>();
    }

    public void Respond()
    {
        container.gameObject.SetActive(false);
        DialogueManager.Instance.OnRespond(index);

        if (gameObject != null) Destroy(gameObject, .1f);
        Debugger.Instance.CreateLog("Choice Clicked");
    }
}
