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
            if (stunTimer <= 0)
            {
                stunned = false;
                agent.isStopped = false;

                // Al recuperar: monstruo vuelve a modo pasivo (fase 0 temporal)
                isPassive = true;
                currentPhase = 0;
                uvHits = 0;

                Debug.Log("MONSTER: Recuperó control y vuelve a patrullar (modo pasivo)");
            }
            return;
        }

        // --- MODO PASIVO → patrulla y NO persigue, NO mata ---
        if (isPassive)
        {
            Patrol();
            return;
        }

        // --- MODO ACTIVO (fase 1, 2, 3) ---
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
            agent.SetDestination(player.position);
        else
            Patrol();
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
    public void ApplyUV()
    {
        // En fase pasiva, UV no debería hacer nada
        if (isPassive)
        {
            Debug.Log("UV no afecta al monstruo en modo pasivo");
            return;
        }

        // Fase 1 → stun directo
        if (currentPhase == 1)
        {
            Debug.Log("UV → stun directo (fase 1)");
            ApplyStun(uvStunTime);
            return;
        }

        // Fase 2 → necesita 2 golpes UV
        if (currentPhase == 2)
        {
            uvHits++;
            Debug.Log("UV HIT fase 2: " + uvHits);

            if (uvHits >= 2)
            {
                ApplyStun(uvStunTime);
                uvHits = 0;
            }

            return;
        }

        // Fase 3 → no funciona UV
        if (currentPhase == 3)
        {
            Debug.Log("UV no afecta al monstruo en fase 3");
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
            Debug.Log("Monstruo stuneado por trampa");
            ApplyStun(trapStunTime);
        }
        else
        {
            Debug.Log("Trampa no afecta en fase 3");
        }
    }

    void ApplyStun(float time)
    {
        stunned = true;
        agent.isStopped = true;
        stunTimer = time;

        Debug.Log("MONSTER: STUN por " + time + "s");
    }

    // ===============================
    //     FASES CONTROLADAS POR EL GAME MANAGER
    // ===============================
    public void SetPhase(int phase)
    {
        currentPhase = phase;
        uvHits = 0;

        // Fase 0 (patrullar)
        if (phase == 0)
        {
            isPassive = true;
            agent.speed = 2f;
            Debug.Log("MONSTER → Modo pasivo (Fase 0)");
            return;
        }

        // Fase activa
        isPassive = false;

        if (phase == 1)
            agent.speed = 3f;
        else if (phase == 2)
            agent.speed = 4.5f;
        else if (phase == 3)
            agent.speed = 6f;

        Debug.Log("MONSTER → Fase " + phase + " (activo)");
    }

    // ===============================
    //       ATAQUE AL JUGADOR
    // ===============================
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        // Sólo puede matar si está activo (fase 1, 2, 3)
        if (health != null && !isPassive)
        {
            Debug.Log("MONSTER: Jugador atrapado → muerte inmediata");
            health.KillPlayer();
        }
        else if (health != null)
        {
            Debug.Log("MONSTER TOCA AL PLAYER, pero está en modo pasivo");
        }
    }
}
