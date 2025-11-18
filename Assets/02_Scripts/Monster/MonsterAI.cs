using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] patrolPoints;

    int currentPoint = 0;
    int phase = 0;

    bool canBeStunned_ByUV = false;
    bool canBeStunned_ByTrap = false;

    bool stunned = false;
    float stunTimer = 0f;

    public Transform player;
    public float chaseDistance = 10f;

    void Update()
    {
        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                stunned = false;
                agent.isStopped = false;
            }
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < chaseDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    public void SetPhase(int newPhase)
    {
        phase = newPhase;

        if (phase == 1)
        {
            agent.speed = 3f;
            canBeStunned_ByUV = true;
            canBeStunned_ByTrap = true;
        }
        if (phase == 2)
        {
            agent.speed = 4f;
            canBeStunned_ByUV = false;
            canBeStunned_ByTrap = true;
        }
        if (phase == 3)
        {
            agent.speed = 6f;
            canBeStunned_ByUV = false;
            canBeStunned_ByTrap = false;
        }

        Debug.Log("Monstruo fase " + phase);
    }

    public void ApplyUV()
    {
        if (!canBeStunned_ByUV) return;
        Stun(2f);
    }

    public void ApplyTrap()
    {
        if (!canBeStunned_ByTrap) return;
        Stun(3f);
    }

    void Stun(float time)
    {
        stunned = true;
        stunTimer = time;
        agent.isStopped = true;
        Debug.Log("Monstruo stuneado " + time + "s");
    }
}
