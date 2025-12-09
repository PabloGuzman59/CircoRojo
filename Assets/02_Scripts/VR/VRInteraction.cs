using UnityEngine;
using UnityEngine.InputSystem;

public class VRInteraction : MonoBehaviour
{
    public Transform hand;             // Mano derecha
    public float distance = 3f;
    public PlayerInventory inv;

    public InputActionReference interactButton;  // botón A, o PS5 CROSS

    void Update()
    {
        if (interactButton.action.WasPressedThisFrame())
        {
            if (Physics.Raycast(hand.position, hand.forward, out RaycastHit hit, distance))
            {
                var interact = hit.collider.GetComponent<SimpleInteractable>();
                if (interact != null)
                {
                    interact.OnInteract(inv);
                }
            }
        }
    }
}
