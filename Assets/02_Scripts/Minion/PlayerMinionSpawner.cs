using UnityEngine;
using UnityEngine.AI;

public class PlayerMinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Transform player;

    public float spawnInterval = 12f;
    public int maxMinions = 4;

    public float spawnRadius = 8f;
    public float minDistance = 4f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0;
            TrySpawnNearPlayer();
        }
    }

    void TrySpawnNearPlayer()
    {
        if (FindObjectsOfType<MinionAI>().Length >= maxMinions)
            return;

        Vector3 randomPos = Vector3.zero;
        bool found = false;

        for (int i = 0; i < 30; i++)
        {
            Vector2 circle = Random.insideUnitCircle * spawnRadius;

            Vector3 potential = new Vector3(
                player.position.x + circle.x,
                player.position.y,
                player.position.z + circle.y
            );

            // evitar spawn demasiado cerca
            if (Vector3.Distance(player.position, potential) < minDistance)
                continue;

            // comprobar si está sobre navmesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(potential, out hit, 1.5f, NavMesh.AllAreas))
            {
                randomPos = hit.position;
                found = true;
                break;
            }
        }

        if (found)
        {
            Instantiate(minionPrefab, randomPos, Quaternion.identity);
            Debug.Log("PLAYER SPAWNER → Minion creado cerca del jugador");
        }
        else
        {
            Debug.Log("PLAYER SPAWNER → No se encontró un punto válido.");
        }
    }
}
