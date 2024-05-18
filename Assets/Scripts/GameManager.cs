using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static Debugger dbgr;

    private void Awake()
    {
        if (!Instance) Instance = this;
        if (!dbgr) dbgr = gameObject.AddComponent<Debugger>();

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
    } 

}
