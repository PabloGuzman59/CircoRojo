using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform cam;
    public float distance = 3f;
    public PlayerInventory inv;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, distance))
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
