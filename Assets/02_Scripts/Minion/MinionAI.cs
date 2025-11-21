using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public Transform[] patrolPoints;
    private int currentPoint = 0;

    public float detectionRange = 8f;

    private bool dead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (dead) return;

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

        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    public void ApplyUV()
    {
        if (dead) return;

        Debug.Log("MINION: Destruido por luz UV.");
        dead = true;

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si toca al jugador, lo ralentiza
        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            StartCoroutine(SlowPlayer(pm));
        }
    }

    System.Collections.IEnumerator SlowPlayer(PlayerMovement pm)
    {
        Debug.Log("MINION: Ralentizando jugador...");
        float originalSpeed = pm.walkSpeed;
        pm.walkSpeed = 2f;

        yield return new WaitForSeconds(2f);

        pm.walkSpeed = originalSpeed;
        Debug.Log("MINION: Efecto terminado.");
    }
}
