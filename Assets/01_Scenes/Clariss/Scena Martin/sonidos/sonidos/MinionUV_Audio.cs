using UnityEngine;

public class MinionUV_Audio : MonoBehaviour
{
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip uvAnnoyedScream;

    private bool wasUnderUV = false;

    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || audioSource == null || uvAnnoyedScream == null) return;

        bool underUV = animator.GetBool("UnderUV");

        // al entrar en UV (false -> true) suena una vez
        if (underUV && !wasUnderUV)
        {
            audioSource.PlayOneShot(uvAnnoyedScream);
        }

        wasUnderUV = underUV;
    }
}
