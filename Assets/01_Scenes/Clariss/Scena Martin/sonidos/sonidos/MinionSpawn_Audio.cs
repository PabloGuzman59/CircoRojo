using UnityEngine;

public class MinionSpawn_Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip spawnSound;

    private int lastMinionCount = 0;

    void Start()
    {
        lastMinionCount = FindObjectsOfType<MinionAI>().Length;
    }

    void Update()
    {
        if (audioSource == null || spawnSound == null) return;

        int currentCount = FindObjectsOfType<MinionAI>().Length;

        // Si aumentó el número, apareció uno nuevo
        if (currentCount > lastMinionCount)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        lastMinionCount = currentCount;
    }
}
