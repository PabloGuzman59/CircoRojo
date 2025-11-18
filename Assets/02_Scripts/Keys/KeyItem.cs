using UnityEngine;

public class KeyItem : SimpleInteractable
{
    public int keyId = 1;

    public override void OnInteract(PlayerInventory inv)
    {
        if (keyId == 1) inv.key1 = true;
        if (keyId == 2) inv.key2 = true;
        if (keyId == 3) inv.key3 = true;
        if (keyId == 4) inv.key4 = true;

        Debug.Log("Recogiste llave " + keyId);
        // Aquí la UI actualizaría inventario
        Destroy(gameObject);
    }
}
