using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public Transform[] patrolPoints;
    private int currentPoint = 0;

    public float detectionRange = 10f;

    private bool stunned = false;
    private float stunTimer = 0f;

    private int currentPhase = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

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

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
        {
            // Persigue al jugador
            agent.SetDestination(player.position);
        }
        else
        {
            // Patrulla
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    public void ApplyUV()
    {
        if (currentPhase == 1)
        {
            Stun(2f);
            Debug.Log("Monstruo stuneado por UV (fase 1)");
        }
    }

    public void ApplyTrap()
    {
        if (currentPhase <= 2)
        {
            Stun(3f);
            Debug.Log("Monstruo stuneado por trampa");
        }
    }

    void Stun(float time)
    {
        stunned = true;
        stunTimer = time;
        agent.isStopped = true;
    }

    public void SetPhase(int phase)
    {
        currentPhase = phase;

        if (phase == 1)
        {
            agent.speed = 3f;
        }
        else if (phase == 2)
        {
            agent.speed = 4.5f;
        }
        else if (phase == 3)
        {
            agent.speed = 6f;
        }

        Debug.Log("Monstruo ahora está en fase " + phase);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("MONSTRUO: El jugador ha sido atrapado.");
            playerHealth.KillPlayer();
        }
    }

}
