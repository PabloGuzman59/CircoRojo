using UnityEngine;
using UnityEngine.AI;

public class MonsterBase_Audio : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip baseLoop; // respiración + pasos (un solo loop)

    private bool wasActive = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (audioSource == null || baseLoop == null) return;

        // Estado base: caminando pero no eufórico y no stuneado y no game over
        bool isWalking = (agent != null && agent.velocity.magnitude > 0.05f);
        bool euphoric = animator != null && animator.GetBool("IsEuphoric");
        bool stunned = animator != null && animator.GetBool("UnderUV");
        bool gameOver = animator != null && animator.GetBool("IsScared");

        bool shouldPlay = isWalking && !euphoric && !stunned && !gameOver;

        if (shouldPlay && !wasActive)
        {
            audioSource.clip = baseLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!shouldPlay && wasActive)
        {
            audioSource.Stop();
        }

        wasActive = shouldPlay;
    }
}
