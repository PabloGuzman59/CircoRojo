using UnityEngine;

public class PlayerMovement_Audio : MonoBehaviour
{
    public PlayerMovement movement;
    public AudioSource audioSource;
    public AudioClip footsteps;

    private bool wasMoving = false;

    void Update()
    {
        if (movement == null || audioSource == null || footsteps == null)
            return;

        // Detectar movimiento (WASD)
        bool isMoving =
            Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        // Empezar pasos
        if (isMoving && !wasMoving)
        {
            audioSource.clip = footsteps;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Detener pasos
        if (!isMoving && wasMoving)
        {
            audioSource.Stop();
        }

        wasMoving = isMoving;
    }
}
