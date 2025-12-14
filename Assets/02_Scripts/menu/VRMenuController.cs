using UnityEngine;

public class VRMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;      // PanelMain
    public GameObject optionsPanel;   // PanelOptions

    [Header("Objects to Remove on Start")]
    public GameObject[] objectsToRemove; // ← aquí pones los 4 objetos

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
        // Ocultar menú
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        // 🔥 Eliminar objetos
        if (objectsToRemove != null)
        {
            foreach (GameObject obj in objectsToRemove)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        // Aquí puedes activar HUD, scripts del jugador, etc.
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
