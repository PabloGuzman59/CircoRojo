using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead = false;

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER: Has sido atrapado por el monstruo.");

        // Bloquear movimiento
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerLook>().enabled = false;

        // Más adelante: bloquear UI, sonidos, animación, jumpscare
        Debug.Log("GAME OVER: Movimiento y cámara desactivados.");

        // Llamar al GameManager
        GameManager.instance.PlayerDied();
    }
}
