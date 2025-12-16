using UnityEngine;

public class MonsterStun_Audio : MonoBehaviour
{
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip stunGroan;

    private bool wasUnderUV = false;

    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || audioSource == null || stunGroan == null) return;

        bool underUV = animator.GetBool("UnderUV");

        // al entrar en UV (false -> true) suena una vez
        if (underUV && !wasUnderUV)
        {
            audioSource.PlayOneShot(stunGroan);
        }

        wasUnderUV = underUV;
    }
}
