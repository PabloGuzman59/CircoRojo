using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerCamera;  // La cámara principal del jugador
    public Transform gameOverCameraPosition;  // Posición de la cámara cuando ocurre el game over
    public Transform originalCameraPosition;  // Posición original de la cámara antes del game over
    public float transitionSpeed = 5f;  // Velocidad de la transición entre las posiciones de la cámara

    // Jumpscare settings
    public float jumpscareSpeed = 8f;  // Velocidad de la transición para jumpscare
    public float jumpscareDistance = 0.6f;  // Distancia de acercamiento de la cámara al enemigo
    public AudioSource audioSource;  // Para reproducir sonido
    public AudioClip jumpscareSound;  // Sonido para el jumpscare

    private bool isGameOver = false;
    private bool isJumpscareActive = false;
    private Transform targetEnemy;  // El enemigo que está causando el jumpscare

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isJumpscareActive && targetEnemy != null)
        {
            // Acercamos la cámara al enemigo
            Vector3 targetPos = targetEnemy.position + targetEnemy.forward * jumpscareDistance;
            playerCamera.position = Vector3.Lerp(playerCamera.position, targetPos, jumpscareSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(targetEnemy.position - playerCamera.position);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, jumpscareSpeed * Time.deltaTime);
        }
        else if (isGameOver)
        {
            // Mover cámara al game over
            playerCamera.position = Vector3.Lerp(playerCamera.position, gameOverCameraPosition.position, transitionSpeed * Time.deltaTime);
            playerCamera.rotation = Quaternion.Lerp(playerCamera.rotation, gameOverCameraPosition.rotation, transitionSpeed * Time.deltaTime);
        }
        else
        {
            // Volver a la posición original
            playerCamera.position = Vector3.Lerp(playerCamera.position, originalCameraPosition.position, transitionSpeed * Time.deltaTime);
            playerCamera.rotation = Quaternion.Lerp(playerCamera.rotation, originalCameraPosition.rotation, transitionSpeed * Time.deltaTime);
        }
    }

    // Llamado por Minion o Monster para activar el Game Over
    public void TriggerGameOver()
    {
        isGameOver = true;
        isJumpscareActive = false; // Aseguramos que el jumpscare no se activa durante Game Over

        // Reproducir sonido de game over si está asignado
        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound);
    }

    // Llamado para activar el jumpscare cuando un enemigo se acerque
    public void TriggerJumpscare(Transform enemy)
    {
        isJumpscareActive = true;
        targetEnemy = enemy;

        // Reproducir sonido de jumpscare
        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound);

        // Congelar el tiempo para que el jumpscare sea efectivo
        Time.timeScale = 0f;
    }

    // Resetear la cámara después del game over
    public void ResetCamera()
    {
        isGameOver = false;
        isJumpscareActive = false;
        targetEnemy = null;
    }
}
