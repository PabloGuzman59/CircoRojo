using UnityEngine;

public class MapMinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 10f;
    public int maxMinions = 6;

    float timer;

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
        if (FindObjectsOfType<MinionAI>().Length >= maxMinions)
            return;

        // Elegir un punto aleatorio del mapa
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(minionPrefab, sp.position, Quaternion.identity);
        Debug.Log("MAP SPAWNER → Minion creado en punto fijo");
    }
}
