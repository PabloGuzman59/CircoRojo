using UnityEngine;

public class MinionInitializer : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GameObject[] pts = GameObject.FindGameObjectsWithTag("PatrolPoint");
            patrolPoints = new Transform[pts.Length];
            for (int i = 0; i < pts.Length; i++)
                patrolPoints[i] = pts[i].transform;
        }

        MinionAI ai = GetComponent<MinionAI>();
        ai.player = player;
        ai.patrolPoints = patrolPoints;
    }
}