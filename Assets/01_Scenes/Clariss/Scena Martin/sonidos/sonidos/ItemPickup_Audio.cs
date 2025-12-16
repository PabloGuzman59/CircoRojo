using UnityEngine;

public class PlayerInventory_Audio : MonoBehaviour
{
    public PlayerInventory inventory;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    // Estados anteriores
    private bool k1, k2, k3, k4;
    private bool c1, c2, c3;

    void Start()
    {
        if (inventory == null) return;

        k1 = inventory.key1;
        k2 = inventory.key2;
        k3 = inventory.key3;
        k4 = inventory.key4;

        c1 = inventory.card1;
        c2 = inventory.card2;
        c3 = inventory.card3;
    }

    void Update()
    {
        if (inventory == null || audioSource == null || pickupSound == null)
            return;

        // Si alguna key o card pasó de false → true
        if (
            inventory.key1 && !k1 ||
            inventory.key2 && !k2 ||
            inventory.key3 && !k3 ||
            inventory.key4 && !k4 ||
            inventory.card1 && !c1 ||
            inventory.card2 && !c2 ||
            inventory.card3 && !c3
        )
        {
            audioSource.PlayOneShot(pickupSound);
        }

        // Actualizar estados
        k1 = inventory.key1;
        k2 = inventory.key2;
        k3 = inventory.key3;
        k4 = inventory.key4;

        c1 = inventory.card1;
        c2 = inventory.card2;
        c3 = inventory.card3;
    }
}
