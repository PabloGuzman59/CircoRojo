using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public NavMeshAgent agent;

    // Punto de perseguimiento al player
    public Transform player;

    // Puntos de patrulla del minion
    public Transform[] patrolPoints;
    private int currentPoint = 0;

    public float detectionRange = 8f;

    private bool dead = false;

    // ============================
    //  VARIABLES DE LUZ UV
    // ============================
    private bool takingUV = false;
    private float uvTimer = 0f;
    public float uvRequiredTime = 4f; // tiempo necesario bajo UV

    // 🔥 NUEVO: velocidad a la que se pierde el efecto UV
    public float uvDecaySpeed = 1.5f;

    // Animaciones
    private Animator animator;

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
        if (dead) return;

        // ============================================================
        //  LÓGICA DE MUERTE POR LUZ UV
        // ============================================================
        if (takingUV)
        {
            // Si está recibiendo UV, acumula tiempo
            uvTimer += Time.deltaTime;

            agent.isStopped = true;

            animator.SetBool("UnderUV", true);
            animator.SetBool("IsWalking", false);

            if (uvTimer >= uvRequiredTime)
            {
                DieByUV();
                return;
            }
        }
        else
        {
            // 🔥 CLAVE: NO reiniciar el contador de golpe
            if (uvTimer > 0f)
            {
                uvTimer -= uvDecaySpeed * Time.deltaTime;
                uvTimer = Mathf.Max(uvTimer, 0f);
            }

            agent.isStopped = false;
            animator.SetBool("UnderUV", false);
        }

        // ============================================================
        //  MOVIMIENTO NORMAL (PERSEGUIR / PATRULLAR)
        // ============================================================
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
            agent.SetDestination(player.position);
        else
            Patrol();

        // ACTUALIZAR ANIMACIÓN DE CAMINAR (MUY IMPORTANTE)
        bool isMoving = agent.velocity.magnitude > 0.05f;
        animator.SetBool("IsWalking", isMoving);
    }

    void DieByUV()
    {
        if (dead) return;
        dead = true;

        agent.isStopped = true;

        animator.SetBool("UnderUV", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsScared", true);  // animación de muerte

        Debug.Log("MINION MUERTO POR LUZ UV TRAS 4 SEGUNDOS");

        Destroy(gameObject, 1.2f); // tiempo para animación
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    // ============================================================
    //  INTERFAZ DE LUZ UV (LLAMADA DESDE LA LINTERNA)
    // ============================================================
    public void ApplyUV(bool active)
    {
        if (dead) return;

        takingUV = active;
    }

    // ============================================================
    //  COLISIONES CON PLAYER
    // ============================================================
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MINION: Algo entró → " + other.name);

        PlayerHealthVR health = other.GetComponentInParent<PlayerHealthVR>();
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();

        if (health != null)
        {
            animator.SetBool("IsScared", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsChasing", false);

            CameraControl cameraControl = Camera.main.GetComponent<CameraControl>();
            if (cameraControl != null)
                cameraControl.TriggerGameOver();

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
    //  Matar jugador luego de 4 segundos
    // ============================================================
    System.Collections.IEnumerator KillAfterSeconds(PlayerHealthVR health)
    {
        yield return new WaitForSeconds(4f);
        health.KillPlayer();
    }
}
