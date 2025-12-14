using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerCamera;
    public Transform gameOverCameraPosition;  // Puedes asignar la posición de cámara en el editor
    public Transform originalCameraPosition;

    public float transitionSpeed = 5f; // Ajusta la velocidad del movimiento de la cámara
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver)
        {
            // Hacer que la cámara se mueva hacia la posición de Game Over
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

    public void TriggerGameOver()
    {
        isGameOver = true;
    }

    public void ResetCamera()
    {
        isGameOver = false;
    }
}
