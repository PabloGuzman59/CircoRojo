using UnityEngine;

public class KeyItem : SimpleInteractable
{
    public int keyId = 1; // 1, 2 o 3

    public override void OnInteract(PlayerInventory inv)
    {
        if (keyId == 1) inv.key1 = true;
        if (keyId == 2) inv.key2 = true;
        if (keyId == 3) inv.key3 = true;

        Debug.Log("Recogiste llave " + keyId);

        // Actualizar UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeys(inv);
        }

        Destroy(gameObject);
    }
}