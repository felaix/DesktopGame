using UnityEngine;

public class Debugger : MonoBehaviour
{
    public static Debugger Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
    }
    public void CreateWarningLog(string message) => Debug.LogFormat("<b><color=#FFFF99>Warning: </color></b> <color=white>{0}</color>", message);
    public void CreateErrorLog(string message) => Debug.LogFormat("<b><color=red>Warning: </color></b> <color=white>{0}</color>", message);
    public void CreateLog(string message) => Debug.LogFormat("<b><color=#FFFF99>Message: </color></b> <color=white>{0}</color>", message);

}
