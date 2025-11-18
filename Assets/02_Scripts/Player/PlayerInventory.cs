using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool key1, key2, key3, key4;
    public bool card1, card2, card3;

    public int batteryCount = 0;

    public void AddBattery()
    {
        batteryCount++;
        Debug.Log("Baterías: " + batteryCount);
        // Aquí llamas a la UI para actualizar icono y número
    }
}
