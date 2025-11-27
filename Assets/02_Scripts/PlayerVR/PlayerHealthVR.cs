using UnityEngine;

public class PlayerHealthVR : MonoBehaviour
{
    public bool isDead = false;

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("GAME OVER — El jugador ha muerto.");

        // Aquí luego pondremos la UI
        Time.timeScale = 0f;
    }
}
