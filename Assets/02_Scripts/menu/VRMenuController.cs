using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;      // PanelMain
    public GameObject optionsPanel;   // PanelOptions

    [Header("Scene")]
    public string sceneToLoad = "Scena Martin";

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenOptions()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void BackFromOptions()
    {
        ShowMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
