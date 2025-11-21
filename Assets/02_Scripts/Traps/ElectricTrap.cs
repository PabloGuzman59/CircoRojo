using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    public float stunTime = 3f; // Duración del stun en fase 1 y 2

    private void OnTriggerEnter(Collider other)
    {
        MonsterAI monster = other.GetComponent<MonsterAI>();

        if (monster != null)
        {
            monster.ApplyTrap();
            Debug.Log("¡Monstruo golpeado por trampa eléctrica!");
        }
    }
}
