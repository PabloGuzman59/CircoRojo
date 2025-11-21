using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraTransform;
    public float sensitivity = 150f;

    float xRot = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -80f, 80f);

        // Rotación vertical solo cámara
        cameraTransform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        // Rotación horizontal del Player
        transform.Rotate(Vector3.up * mouseX);
    }
}
