using UnityEngine;
using UnityEngine.InputSystem;

public class VRInteraction : MonoBehaviour
{
    [Header("Raycast")]
    public Transform hand;
    public float distance = 3f;

    [Header("Inventory")]
    public PlayerInventory inv;

    [Header("Input")]
    public InputActionReference interactButton;  // Secondary Button (B)

    private bool wasPressedLastFrame = false;

    void OnEnable()
    {
        if (interactButton != null)
            interactButton.action.Enable();
    }

    void OnDisable()
    {
        if (interactButton != null)
            interactButton.action.Disable();
    }

    void Update()
    {
        bool isPressed = interactButton != null && interactButton.action.ReadValue<float>() > 0.5f;

        if (isPressed && !wasPressedLastFrame)
        {
            Debug.DrawRay(hand.position, hand.forward * distance, Color.cyan, 0.2f);

            if (Physics.Raycast(
                hand.position,
                hand.forward,
                out RaycastHit hit,
                distance,
                ~0,
                QueryTriggerInteraction.Collide   // 👈 CLAVE
            ))
            {
                var interact = hit.collider.GetComponent<SimpleInteractable>();
                if (interact != null)
                {
                    Debug.Log("INTERACTUAR (B) con " + hit.collider.name);
                    interact.OnInteract(inv);
                }
            }
        }

        wasPressedLastFrame = isPressed;
    }
}
