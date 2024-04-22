using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static Debugger dbgr;

    private void Awake()
    {
        if (!Instance) Instance = this;
        if (!dbgr) dbgr = gameObject.AddComponent<Debugger>();
    }

}
