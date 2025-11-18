using UnityEngine;

public class SimpleInteractable : MonoBehaviour
{
    public virtual void OnInteract(PlayerInventory inv)
    {
        Debug.Log("Interactúa con objeto: " + gameObject.name);
    }
}
