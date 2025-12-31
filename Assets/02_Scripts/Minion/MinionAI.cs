using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public NavMeshAgent agent;
    //Punto de perseguimiento al player
    public Transform player;
    //Puntos de patrulla del minion
    public Transform[] patrolPoints;

    private int currentPoint = 0;

    public float detectionRange = 8f;

    private bool dead = false;

    // Animaciones
    private bool takingUV = false;
    private float uvTimer = 0f;
    public float uvRequiredTime = 4f; // tiempo necesario bajo UV



    //Animaciones

    private Animator animator;

    //EFECTOS
    public AudioClip burnSound;
    public Material burnMaterial;           

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (takingUV)
        {
            uvTimer += Time.deltaTime;

            if (uvTimer >= uvRequiredTime)
            {
                DieByUV();
            }

            return; // NO persigue ni patrulla mientras está bajo UV
        }

        if (dead) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
            agent.SetDestination(player.position);
        else
            Patrol();

        // ACTUALIZAR ANIMACIÓN DE CAMINAR (MUY IMPORTANTE)
        if (!takingUV && !dead)
        {
            bool isMoving = agent.velocity.magnitude > 0.05f;
            animator.SetBool("IsWalking", isMoving);
        }
    }
    void DieByUV()
    {
        if (dead) return;
        dead = true;
        // Aplicar sonido de quemado
        if (burnSound != null)
        {
            AudioSource.PlayClipAtPoint(burnSound, transform.position);
        }

        // Aplicar material de quemado
        Renderer rend = GetComponent<Renderer>();
        if (rend != null && burnMaterial != null)
        {
            rend.material = burnMaterial;
        }
        animator.SetBool("UnderUV", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsScared", true);  // puedes usar Scared como estado de muerte si quieres

        Debug.Log("MINION MUERTO POR LUZ UV TRAS 4 SEGUNDOS");

        Destroy(gameObject, 1.2f); // dale tiempo a animación de caída
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
    public void ApplyUV(bool active)
    {
        if (dead) return;

        if (active)
        {
            if (!takingUV)
            {
                takingUV = true;
                uvTimer = 0f;
                agent.isStopped = true;

                animator.SetBool("UnderUV", true);   // ACTIVAR ANIMACIÓN UV
                animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            if (takingUV)
            {
                takingUV = false;
                uvTimer = 0f;
                agent.isStopped = false;

                animator.SetBool("UnderUV", false);  // VOLVER A WALK SI CORRESPONDE
            }
        }
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
            animator.SetBool("IsScared", true); // activa animación Monkey_GameOver
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