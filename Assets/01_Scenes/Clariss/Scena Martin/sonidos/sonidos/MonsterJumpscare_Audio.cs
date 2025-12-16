using UnityEngine;

public class MonsterJumpscare_Audio : MonoBehaviour
{
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip jumpscare;

    private bool wasGameOver = false;

    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || audioSource == null || jumpscare == null) return;

        bool gameOver = animator.GetBool("IsScared"); // o el bool que uses para game over

        // al entrar a game over (false -> true) suena una vez
        if (gameOver && !wasGameOver)
        {
            audioSource.PlayOneShot(jumpscare);
        }

        wasGameOver = gameOver;
    }
}
