using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public int maxMinions = 4;
    public float spawnInterval = 8f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0;
            TrySpawn();
        }
    }

    void TrySpawn() 
    {
        if (GameObject.FindObjectsOfType<MinionAI>().Length >= maxMinions)
            return;

        Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Debug.Log("Spawner: Minion creado.");
    }
}
