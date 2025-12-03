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
            agent.SetDestination(player.position);
        else
            Patrol();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    // ============================================================
    //  MUERTE POR LUZ UV
    // ============================================================
    public void ApplyUV()
    {
        if (dead) return;

        Debug.Log("MINION: Destruido por luz UV.");
        dead = true;
        Destroy(gameObject);
    }

    // ============================================================
    //  COLISIONES CON PLAYER (3D)
    // ============================================================
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MINION: Algo entró → " + other.name);
        // 1. Buscar PlayerHealth
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        // 2. Buscar PlayerMovement
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();

        if (health != null)
        {
            Debug.Log("MINION: Tocando al jugador → muerte en 4 segundos.");
            StartCoroutine(KillAfterSeconds(health));
        }

        if (movement != null)
        {
            Debug.Log("MINION: Ralentizando jugador...");
            StartCoroutine(SlowPlayer(movement));
        }
    }

    // ============================================================
    //  Ralentizar jugador
    // ============================================================
    System.Collections.IEnumerator SlowPlayer(PlayerMovement pm)
    {
        float originalSpeed = pm.walkSpeed;
        pm.walkSpeed = 2f;

        yield return new WaitForSeconds(2f);

        pm.walkSpeed = originalSpeed;
        Debug.Log("MINION: Ralentización terminada.");
    }

    // ============================================================
    //  Matar luego de 4 segundos
    // ============================================================
    System.Collections.IEnumerator KillAfterSeconds(PlayerHealth health)
    {
        yield return new WaitForSeconds(4f);
        health.KillPlayer();
    }
}
