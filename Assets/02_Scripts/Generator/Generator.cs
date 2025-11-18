using UnityEngine;

public class Generator : SimpleInteractable
{
    public int generatorId = 1;
    public int requiredCard = 1;

    private bool activated = false;

    public override void OnInteract(PlayerInventory inv)
    {
        if (activated)
        {
            Debug.Log("Generador ya está activado.");
            return;
        }

        bool hasCard = (requiredCard == 1 && inv.card1) ||
                       (requiredCard == 2 && inv.card2) ||
                       (requiredCard == 3 && inv.card3);

        if (!hasCard)
        {
            Debug.Log("Necesitas la tarjeta " + requiredCard);
            return;
        }

        activated = true;
        Debug.Log("Generador " + generatorId + " ACTIVADO");

        // Aquí llamas a la UI para mostrar progreso
        GameManager.instance.OnGeneratorActivated(generatorId);
    }
}
