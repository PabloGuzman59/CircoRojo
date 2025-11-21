using UnityEngine;

public class Generator : SimpleInteractable
{
    public int generatorId = 1;   // Identificador del generador
    public int requiredCard = 1;  // ¿Qué tarjeta necesita?

    private bool activated = false;

    public override void OnInteract(PlayerInventory inv)
    {
        if (activated)
        {
            Debug.Log("Generador " + generatorId + " ya está activado.");
            return;
        }

        bool hasCard =
            (requiredCard == 1 && inv.card1) ||
            (requiredCard == 2 && inv.card2) ||
            (requiredCard == 3 && inv.card3);

        if (!hasCard)
        {
            Debug.Log("Necesitas la tarjeta " + requiredCard + " para activar este generador.");
            return;
        }

        // ACTIVAR EL GENERADOR
        activated = true;
        Debug.Log("Generador " + generatorId + " ACTIVADO");

        // (Opcional) Cambiar color para indicar activado
        GetComponent<Renderer>().material.color = Color.green;

        // Notificar al GameManager
        GameManager.instance.OnGeneratorActivated(generatorId);
    }
}
