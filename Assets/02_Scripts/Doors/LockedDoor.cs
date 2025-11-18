using UnityEngine;

public class LockedDoor : SimpleInteractable
{
    public int requiredKey = 1;

    public override void OnInteract(PlayerInventory inv)
    {
        bool canOpen = (requiredKey == 1 && inv.key1) ||
                       (requiredKey == 2 && inv.key2) ||
                       (requiredKey == 3 && inv.key3) ||
                       (requiredKey == 4 && inv.key4);

        if (canOpen)
        {
            Debug.Log("Puerta abierta " + requiredKey);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Necesitas la llave " + requiredKey);
        }
    }
}
