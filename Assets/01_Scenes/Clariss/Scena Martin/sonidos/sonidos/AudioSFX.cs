using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public static AudioSFX I;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip flashlightClick;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // 🔊 Linterna normal (ON / OFF)
    public void PlayFlashlightClick()
    {
        if (sfxSource == null || flashlightClick == null) return;

        sfxSource.PlayOneShot(flashlightClick);
    }
}
