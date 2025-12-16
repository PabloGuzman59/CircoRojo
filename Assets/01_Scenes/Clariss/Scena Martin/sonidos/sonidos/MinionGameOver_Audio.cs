using UnityEngine;

public class MinionGameOver_Audio : MonoBehaviour
{
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip cymbalsFrenetic;

    private bool wasScared = false;

    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || audioSource == null || cymbalsFrenetic == null) return;

        bool scared = animator.GetBool("IsScared");

        // cuando entra al estado "IsScared" (game over) suena una vez
        if (scared && !wasScared)
        {
            audioSource.PlayOneShot(cymbalsFrenetic);
        }

        wasScared = scared;
    }
}
