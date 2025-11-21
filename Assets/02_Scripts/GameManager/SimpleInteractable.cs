using UnityEngine;

public class SimpleInteractable : MonoBehaviour
{
    public virtual void OnInteract(PlayerInventory inv)
    {
        Debug.Log("Interacción base con " + gameObject.name);
        // Aquí podría llamar a UI
    }
}
