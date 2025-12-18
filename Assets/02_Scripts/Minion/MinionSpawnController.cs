using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MinionSpawnController : MonoBehaviour
{
    public GameObject starObject;        // ⭐ Urna / estrella
    public GameObject minionModel;       // 🐒 Modelo del mono

    public NavMeshAgent agent;
    public MinionAI minionAI;
    public Animator animator;

    // ============================
    // AUDIO DE SPAWN (UN SOLO CLIP)
    // ============================
    public AudioSource audioSource;      // 🔊 AudioSource DEL MONO
    public AudioClip spawnClip;          // 🔊 ÚNICO sonido de spawn

    public float starTime = 5f;
    public float clapTime = 2f;

    void Start()
    {
        // 🔒 Estado inicial
        if (starObject) starObject.SetActive(true);
        if (minionModel) minionModel.SetActive(false);

        if (agent) agent.enabled = false;
        if (minionAI) minionAI.enabled = false;

        // 🔊 Reproducir sonido de spawn (urna activa)
        if (audioSource && spawnClip)
        {
            audioSource.clip = spawnClip;
            audioSource.loop = true;   // 🔁 mientras dura la urna
            audioSource.Play();
        }

        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        // ⏳ Tiempo de urna
        yield return new WaitForSeconds(starTime);

        // 🐒 Aparece el mono
        if (minionModel) minionModel.SetActive(true);

        // 🔊 Cortar loop y reproducir una vez más (impacto)
        if (audioSource && spawnClip)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.PlayOneShot(spawnClip);
        }

        // ⏳ Esperar aplauso
        yield return new WaitForSeconds(clapTime);

        // ❌ Quitar urna
        if (starObject) starObject.SetActive(false);

        // ✅ Activar IA
        if (agent) agent.enabled = true;
        if (minionAI) minionAI.enabled = true;
    }
}
