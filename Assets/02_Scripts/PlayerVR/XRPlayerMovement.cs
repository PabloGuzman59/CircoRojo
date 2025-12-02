using UnityEngine;

public class XRPlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public Transform head; // Main Camera

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // dirección basada en la cámara
        Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
        Vector3 right = new Vector3(head.right.x, 0, head.right.z).normalized;

        Vector3 move = forward * z + right * x;

        // Mover XR Origin directamente
        transform.position += move * speed * Time.deltaTime;
    }
}
