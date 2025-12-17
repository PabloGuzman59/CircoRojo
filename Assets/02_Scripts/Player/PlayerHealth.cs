using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead = false;
    public GameOverManager gameOverManager;

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER: Has sido atrapado por el monstruo.");

        // Bloquear movimiento
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerLook>().enabled = false;

        // Bloquear cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        // Llamar al GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.PlayerDied();
        }

        // Activar Game Over con video
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager no está asignado en PlayerHealth!");
        }
    }
}
//using UnityEngine;

//public class PlayerHealth : MonoBehaviour
//{
//    public bool isDead = false;

//    public void KillPlayer()
//    {
//        if (isDead) return;

//        isDead = true;
//        Debug.Log("PLAYER: Has sido atrapado por el monstruo.");

//        // Bloquear movimiento
//        GetComponent<PlayerMovement>().enabled = false;
//        GetComponent<PlayerLook>().enabled = false;

//        // Más adelante: bloquear UI, sonidos, animación, jumpscare
//        Debug.Log("GAME OVER: Movimiento y cámara desactivados.");

//        // Llamar al GameManager
//        GameManager.instance.PlayerDied();
//    }
//}
