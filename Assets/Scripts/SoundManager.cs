using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundManager Instance {  get; private set; }

    private void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(Instance);
    }
}
