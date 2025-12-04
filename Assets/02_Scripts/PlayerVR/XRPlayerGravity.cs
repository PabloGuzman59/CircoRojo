using UnityEngine;

public class XRPlayerGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Vector3 velocity;

    void Update()
    {
        // Detectar suelo con raycast
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;  // pegado al suelo

        // aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
    }
}
