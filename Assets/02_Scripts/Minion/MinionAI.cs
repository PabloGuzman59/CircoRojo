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
    private Coroutine killCoroutine;
    private bool dead = false;

    // ============================
    //  VARIABLES DE LUZ UV
    // ============================
    private bool takingUV = false;
    private float uvTimer = 0f;
    public float uvRequiredTime = 2f; // tiempo necesario bajo UV

    // 🔥 velocidad a la que se pierde el efecto UV
    public float uvDecaySpeed = 1.5f;

    // Animaciones
    private Animator animator;

    // ============================
    //  SONIDOS DEL MINION
    // ============================
    public AudioSource audioSource;

    public AudioClip walkClip;        // monoCaminando.mp3
    public AudioClip uvClip;          // MonoUV.mp3
    public AudioClip attackClip;      // MonoEnojado.mp3
    public AudioClip deathClip;       // monoDesintegrarse.mp3

    // Material de muerte
    public Material burnMaterial;
    private SkinnedMeshRenderer meshRenderer;

    private bool wasWalking = false;
    private bool wasUnderUV = false;

    // ============================
    //  ATAQUE: SLOW + MUERTE
    // ============================
    public float slowSpeed = 2f;        // velocidad del jugador atrapado
    public float slowDuration = 7f;     // cuánto dura la ralentización
    public float killDelay = 7f;         // tiempo total hasta matar

    private bool isAttacking = false;    // 🔒 evita muerte instantánea

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
            uvTimer += Time.deltaTime;
            agent.isStopped = true;

            animator.SetBool("UnderUV", true);
            animator.SetBool("IsWalking", false);

            // 🔊 sonido UV
            if (!wasUnderUV)
            {
                PlayLoop(uvClip);
                wasUnderUV = true;
            }

            if (uvTimer >= uvRequiredTime)
            {
                DieByUV();
                return;
            }
        }
        else
        {
            if (uvTimer > 0f)
            {
                uvTimer -= uvDecaySpeed * Time.deltaTime;
                uvTimer = Mathf.Max(uvTimer, 0f);
            }

            animator.SetBool("UnderUV", false);
            agent.isStopped = false;

            if (wasUnderUV)
            {
                StopSound();
                wasUnderUV = false;
            }
        }

        // ============================================================
        //  MOVIMIENTO NORMAL
        // ============================================================
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
            agent.SetDestination(player.position);
        else
            Patrol();

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);

        // 🔊 sonido de pasos
        if (isMoving && !takingUV && !wasWalking)
        {
            PlayLoop(walkClip);
            wasWalking = true;
        }
        else if (!isMoving && wasWalking)
        {
            StopSound();
            wasWalking = false;
        }
    }

    // ============================================================
    //  MUERTE POR UV
    // ============================================================
    void DieByUV()
    {
        if (dead) return;
        dead = true;

        agent.isStopped = true;
        StopSound();

        // 🔊 sonido de muerte
        PlayOneShot(deathClip);

        // 🔥 material quemado
        if (meshRenderer && burnMaterial)
            meshRenderer.material = burnMaterial;

        animator.SetBool("UnderUV", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsScared", true);

        Debug.Log("MINION MUERTO POR LUZ UV");

        Destroy(gameObject, 1.5f);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 1f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    // ============================================================
    //  INTERFAZ DE LUZ UV
    // ============================================================
    public void ApplyUV(bool active)
    {
        if (dead) return;
        takingUV = active;
    }

    // ============================================================
    //  ATAQUE AL JUGADOR (NO MUERE AL TOQUE)
    // ============================================================
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthVR health = other.GetComponentInParent<PlayerHealthVR>();
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();

        if (health == null || dead) return;
        if (isAttacking) return;

        isAttacking = true;

        Debug.Log("MINION: Jugador atrapado, iniciando cuenta regresiva");

        animator.SetBool("IsScared", true);
        animator.SetBool("IsWalking", false);

        PlayOneShot(attackClip);

        if (movement != null)
            StartCoroutine(SlowPlayerForSeconds(movement, slowSpeed, slowDuration));

        // ⏱ guardar referencia
        killCoroutine = StartCoroutine(KillAfterSecondsRealtime(health, killDelay));
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerHealthVR health = other.GetComponentInParent<PlayerHealthVR>();
        if (health == null) return;

        Debug.Log("MINION: Jugador escapó → cancelar ataque");

        if (killCoroutine != null)
            StopCoroutine(killCoroutine);

        isAttacking = false;
    }


    // ============================================================
    //  Ralentizar jugador
    // ============================================================
    System.Collections.IEnumerator SlowPlayerForSeconds(PlayerMovement pm, float newSpeed, float seconds)
    {
        float originalSpeed = pm.walkSpeed;
        pm.walkSpeed = newSpeed;

        yield return new WaitForSecondsRealtime(seconds);

        pm.walkSpeed = originalSpeed;
    }

    // ============================================================
    //  Matar jugador luego de X segundos (Realtime)
    // ============================================================
    System.Collections.IEnumerator KillAfterSecondsRealtime(PlayerHealthVR health, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        if (health != null && !health.isDead)
        {
            Debug.Log("MINION: Tiempo cumplido → matar jugador");
            health.KillPlayer(); // 🔥 aquí se lanza el video vía GameOverManager
        }
    }

    // ============================
    //  HELPERS DE AUDIO
    // ============================
    void PlayLoop(AudioClip clip)
    {
        if (!audioSource || !clip) return;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayOneShot(AudioClip clip)
    {
        if (!audioSource || !clip) return;
        audioSource.PlayOneShot(clip);
    }

    void StopSound()
    {
        if (!audioSource) return;
        audioSource.Stop();
        audioSource.loop = false;
    }
}
