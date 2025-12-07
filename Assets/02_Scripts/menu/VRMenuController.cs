using UnityEngine;

public class VRMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;      // PanelMain
    public GameObject optionsPanel;   // PanelOptions

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
        // 👉 Ya NO cargas ninguna escena.
        // Simplemente ocultas el menú.
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        // Aquí puedes activar HUD, scripts del jugador, etc. si lo necesitas
        // playerController.enabled = true;  ← ejemplo
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
