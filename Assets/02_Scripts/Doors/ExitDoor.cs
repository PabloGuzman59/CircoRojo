using UnityEngine;

public class ExitDoor : SimpleInteractable
{
    bool unlocked = false;

    public void Unlock()
    {
        unlocked = true;
        Debug.Log("La puerta final está desbloqueada");
    }

    public override void OnInteract(PlayerInventory inv)
    {
        if (!unlocked)
        {
            Debug.Log("Faltan generadores.");
            return;
        }

        Debug.Log("ESCAPASTE. FIN DEL JUEGO.");
        // Aquí mostrarías UI de victoria
    }
}
