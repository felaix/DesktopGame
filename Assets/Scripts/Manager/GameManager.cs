using System.Threading.Tasks;
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

    public async void LoadLevel(int lvl, float delay)
    {
        await Task.Delay((int)(delay*1000));
        SceneManager.LoadScene(lvl);
    } 

}
