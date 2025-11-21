using UnityEngine;

public class ExitDoor : SimpleInteractable
{
    bool unlocked = false;

    // Llamado por el GameManager cuando todos los generadores estén activos
    public void Unlock()
    {
        unlocked = true;
        Debug.Log("EXIT DOOR: La puerta de salida ha sido DESBLOQUEADA.");

        // Opcional: cambiar color para ver que está abierta
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.cyan;
        }

        // Aquí luego podrías reproducir un sonido, animación, etc.
    }

    public override void OnInteract(PlayerInventory inv)
    {
        if (!unlocked)
        {
            Debug.Log("EXIT DOOR: Aún no puedes salir, faltan generadores.");
            return;
        }

        Debug.Log("EXIT DOOR: Has interactuado con la puerta de salida.");
        Debug.Log("VICTORIA: El jugador escapó del nivel.");

        // Avisar al GameManager
        GameManager.instance.PlayerEscaped();

        // Bloquear controles (como en la muerte, pero es victoria)
        var pm = inv.GetComponent<PlayerMovement>();
        var pl = inv.GetComponent<PlayerLook>();

        if (pm != null) pm.enabled = false;
        if (pl != null) pl.enabled = false;

        // Aquí luego: UI de victoria, cambiar de escena, etc.
    }
}
