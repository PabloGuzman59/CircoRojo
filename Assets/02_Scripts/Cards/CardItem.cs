using UnityEngine;

public class CardItem : SimpleInteractable
{
    public int cardId = 1;

    public override void OnInteract(PlayerInventory inv)
    {
        if (cardId == 1) inv.card1 = true;
        if (cardId == 2) inv.card2 = true;
        if (cardId == 3) inv.card3 = true;

        Debug.Log("Recogiste tarjeta " + cardId);
        // Aquí se llamaría a la UI
        Destroy(gameObject);
    }
}
