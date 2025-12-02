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

    // ===========================
    //   MUERTE POR LUZ UV
    // ===========================
    public void ApplyUV()
    {
        if (dead) return;

        Debug.Log("MINION: Destruido por luz UV.");

        dead = true;
        Destroy(gameObject);   // lo elimina de la escena
    }

    // ===========================
    //   COLISIONES CON EL PLAYER
    // ===========================
    private void OnTriggerEnter(Collider other)
    {
        // Detectar Player REAL (BodyCollider)
        XRPlayerMovement move = other.GetComponentInParent<XRPlayerMovement>();
        XRPlayerGravity gravity = other.GetComponentInParent<XRPlayerGravity>();
        PlayerHealthVR health = other.GetComponentInParent<PlayerHealthVR>();

        if (health != null)
        {
            Debug.Log("MINION: Atrapó al jugador → contando 4 segundos para matarlo.");
            StartCoroutine(KillAfterSeconds(health));
        }

        if (move != null)
        {
            Debug.Log("MINION: Ralentizando jugador...");
            StartCoroutine(SlowPlayer(move));
        }
    }

    // ===========================
    //   Ralentizar jugador
    // ===========================
    System.Collections.IEnumerator SlowPlayer(XRPlayerMovement move)
    {
        float originalSpeed = move.speed;

        move.speed = 2f;

        yield return new WaitForSeconds(2f);

        move.speed = originalSpeed;
        Debug.Log("MINION: Efecto terminado.");
    }

    // ===========================
    //   Matar jugador después de 4s
    // ===========================
    System.Collections.IEnumerator KillAfterSeconds(PlayerHealthVR health)
    {
        yield return new WaitForSeconds(4f);
        health.KillPlayer();
    }
}
