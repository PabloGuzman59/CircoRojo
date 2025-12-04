using UnityEngine;

public class MinionInitializer : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;

    void Start()
    {
        MinionAI ai = GetComponent<MinionAI>();
        ai.patrolPoints = patrolPoints;
        ai.player = player;
    }
}
