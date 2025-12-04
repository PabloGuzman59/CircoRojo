using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public PlayerHealthVR health;

    void OnTriggerEnter(Collider other)
    {
        var min = other.GetComponent<MinionAI>();
        if (min != null)
        {
            health.KillPlayer();
        }
    }
}
