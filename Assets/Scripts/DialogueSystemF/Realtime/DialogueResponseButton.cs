using DS;
using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseButton : MonoBehaviour
{
    private Button btn;
    public int index;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Respond);
    }

    public void Respond()
    {
        DialogueManager.Instance.OnRespond(index);
        Destroy(this.gameObject, .1f);
    }
}
