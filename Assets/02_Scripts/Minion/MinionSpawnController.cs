using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MinionSpawnController : MonoBehaviour
{
    public GameObject starObject;      // ⭐
    public GameObject minionModel;     // 🐒
    public NavMeshAgent agent;
    public MinionAI minionAI;
    public Animator animator;

    public float starTime = 5f;

    void Start()
    {
        // Estado inicial
        starObject.SetActive(true);
        minionModel.SetActive(false);

        agent.enabled = false;
        minionAI.enabled = false;

        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        // Esperar con estrella
        yield return new WaitForSeconds(starTime);

        // Mostrar mono → entra en aplaudir (estado inicial del Animator)
        minionModel.SetActive(true);

        // Esperar fin del aplauso
        float clapLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clapLength);

        // Quitar estrella
        starObject.SetActive(false);

        // Activar movimiento
        agent.enabled = true;
        minionAI.enabled = true;
    }
}