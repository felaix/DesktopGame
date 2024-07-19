using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{

    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitGameBtn;
    [SerializeField] private Button creditMenuBtn;

    [SerializeField] private GameObject credits;

    private void Start()
    {
        if (mainMenuBtn != null) mainMenuBtn.onClick.AddListener(() => InvokeMainMenu());
        if (quitGameBtn != null) quitGameBtn.onClick.AddListener(() => QuitGame());
        if (creditMenuBtn != null) creditMenuBtn.onClick.AddListener(() => ShowCredits());
    }

    private void ShowCredits()
    {
        Transform container = transform.parent.parent;
        Instantiate(credits, container);
        gameObject.SetActive(transform.parent);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void InvokeMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
