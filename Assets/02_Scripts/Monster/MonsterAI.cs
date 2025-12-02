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
        agent.updateRotation = true;
        agent.updatePosition = true;

        // ir al primer punto
        if (patrolPoints.Length > 0) SetNextPatrolPoint();
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
            agent.SetDestination(player.position);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            SetNextPatrolPoint();
        }
    }

    void SetNextPatrolPoint()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(patrolPoints[currentPoint].position, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("Patrol point fuera del NavMesh: " + patrolPoints[currentPoint].name);
        }
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
            agent.speed = 3f;
        else if (phase == 2)
            agent.speed = 4.5f;
        else if (phase == 3)
            agent.speed = 6f;

        Debug.Log("Monstruo ahora está en fase " + phase);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthVR health = other.GetComponentInParent<PlayerHealthVR>();

        if (health != null)
        {
            Debug.Log("MONSTRUO: atrapó al jugador.");
            health.KillPlayer();
        }
    }
}
