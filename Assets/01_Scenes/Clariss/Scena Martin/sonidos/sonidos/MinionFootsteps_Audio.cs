using UnityEngine;
using UnityEngine.AI;

public class MinionFootsteps_Audio : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip footstepsLoop;

    private bool wasWalking = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (audioSource == null || footstepsLoop == null || agent == null) return;

        bool isWalking = agent.velocity.magnitude > 0.05f;

        if (isWalking && !wasWalking)
        {
            audioSource.clip = footstepsLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!isWalking && wasWalking)
        {
            audioSource.Stop();
        }

        wasWalking = isWalking;
    }
}