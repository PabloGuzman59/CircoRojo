using UnityEngine;

public class BatteryItem : SimpleInteractable
{
    public override void OnInteract(PlayerInventory inv)
    {
        inv.AddBattery();
        Debug.Log("Pila recogida");
        Destroy(gameObject);
    }
}
