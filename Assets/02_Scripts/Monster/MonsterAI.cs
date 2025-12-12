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

    // Estado interno (patrulla / agresivo)
    private bool isPassive = true;

    // UV hits fase 2
    private int uvHits = 0;

    public float uvStunTime = 2f;
    public float trapStunTime = 3f;

    // ============================
    //  ANIMATOR
    // ============================
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // Busca animador automáticamente
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
            SetNextPatrolPoint();
    }

    void Update()
    {
        // --- Mientras está stuneado ---
        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            animator.SetBool("isStunned", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);

            if (stunTimer <= 0)
            {
                stunned = false;
                agent.isStopped = false;

                animator.SetBool("isStunned", false);

                // Al recuperar: monstruo vuelve a modo pasivo (fase 0 temporal)
                isPassive = true;
                currentPhase = 0;
                uvHits = 0;
            }
            return;
        }

        // --- MODO PASIVO → patrulla ---
        if (isPassive)
        {
            Patrol();
            UpdateAnimations();
            return;
        }

        // --- MODO ACTIVO ---
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
            agent.SetDestination(player.position);
        else
            Patrol();

        UpdateAnimations();
    }

    // ===============================
    //       ANIMACIONES
    // ===============================
    void UpdateAnimations()
    {
        bool walking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", walking);

        bool chasing = (!isPassive && !stunned);
        animator.SetBool("isChasing", chasing);
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
            agent.SetDestination(hit.position);
    }

    // ===============================
    //              UV
    // ===============================
    public void ApplyUV(bool active)
    {
        if (active)
        {
            if (isPassive)
            {
                Debug.Log("UV no afecta al monstruo en modo pasivo");
                return;
            }

            if (currentPhase == 1)
            {
                ApplyStun(uvStunTime);
                return;
            }

            if (currentPhase == 2)
            {
                uvHits++;

                if (uvHits >= 2)
                {
                    ApplyStun(uvStunTime);
                    uvHits = 0;
                }
                return;
            }

            if (currentPhase == 3)
            {
                Debug.Log("UV no afecta al monstruo en fase 3");
            }
        }
    }

    // ===============================
    //          TRAMPAS
    // ===============================
    public void ApplyTrap()
    {
        if (isPassive) return;

        if (currentPhase <= 2)
        {
            ApplyStun(trapStunTime);
        }
    }

    void ApplyStun(float time)
    {
        stunned = true;
        agent.isStopped = true;
        stunTimer = time;

        animator.SetBool("isStunned", true);
    }

    // ===============================
    //     FASES CONTROLADAS POR EL GAME MANAGER
    // ===============================
    public void SetPhase(int phase)
    {
        currentPhase = phase;
        uvHits = 0;

        if (phase == 0)
        {
            isPassive = true;
            agent.speed = 2f;
            return;
        }

        isPassive = false;

        if (phase == 1)
            agent.speed = 3f;
        else if (phase == 2)
            agent.speed = 4.5f;
        else if (phase == 3)
            agent.speed = 6f;
    }

    // ===============================
    //       ATAQUE AL JUGADOR
    // ===============================
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (health != null && !isPassive)
        {
            animator.SetBool("isDead", true); // animación game over
            health.KillPlayer();
        }
    }
}
