using UnityEngine;

public class PlayerHealthVR : MonoBehaviour
{
    public bool isDead = false;

    public static PlayerHealthVR Instance;

    void Awake()
    {
        Instance = this;
    }

    public void KillPlayer()
    {
        if (isDead)
        {
            Debug.LogWarning("⚠️ KillPlayer ignorado (ya estaba muerto)");
            return;
        }

        isDead = true;

        Debug.LogError("☠️ PLAYER: Muerte registrada");
        Debug.LogError("☠️ StackTrace:\n" + System.Environment.StackTrace);  

        // 🔔 Avisar al GameOverManager
        GameOverManager gom = FindObjectOfType<GameOverManager>();
        if (gom != null)
        {
            Debug.Log("🎬 Llamando a GameOverManager.TriggerGameOver()");
            gom.TriggerGameOver();
        }
        else
        {
            Debug.LogError("❌ GameOverManager NO encontrado en la escena");
        }
    }
}
