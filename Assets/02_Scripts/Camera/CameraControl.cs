using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public float jumpscareDistance = 0.5f;
    public float jumpscareSpeed = 10f;
    public AudioSource audioSource;
    public AudioClip jumpscareSound;

    private bool jumpscareActive = false;
    private Transform enemyTarget;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (!jumpscareActive || enemyTarget == null) return;

        Vector3 targetPos =
            enemyTarget.position +
            enemyTarget.forward * jumpscareDistance +
            Vector3.up * 0.15f;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            jumpscareSpeed * Time.unscaledDeltaTime
        );

        Quaternion lookRot =
            Quaternion.LookRotation(enemyTarget.position - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRot,
            jumpscareSpeed * Time.unscaledDeltaTime
        );
    }

    // 🔥 LLAMADO POR MINION O MONSTER
    public void TriggerJumpscare(Transform enemy)
    {
        if (jumpscareActive) return;

        jumpscareActive = true;
        enemyTarget = enemy;

        if (audioSource && jumpscareSound)
            audioSource.PlayOneShot(jumpscareSound);

        Time.timeScale = 0f;
    }
}
