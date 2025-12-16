using UnityEngine;

public class Generator_Audio : MonoBehaviour
{
    public Generator generator;

    [Header("Generator Sound")]
    public AudioSource generatorSource;   // sonido del generador (3D)
    public AudioClip generatorStart;

    [Header("Monster Euphoric Sound")]
    public AudioSource euphoricSource;    // sonido eufórico (2D o 3D)
    public AudioClip euphoricLoop;

    private bool wasActivated = false;

    void Update()
    {
        if (generator == null) return;

        // Detectar activación del generador (una sola vez)
        if (!wasActivated && generator.enabled)
        {
            // 🔊 Sonido generador (una vez)
            if (generatorSource != null && generatorStart != null)
            {
                generatorSource.PlayOneShot(generatorStart);
            }

            // 🔊 Sonido eufórico del monster (loop)
            if (euphoricSource != null && euphoricLoop != null)
            {
                euphoricSource.clip = euphoricLoop;
                euphoricSource.loop = true;
                euphoricSource.Play();
            }

            wasActivated = true;
        }
    }
}
