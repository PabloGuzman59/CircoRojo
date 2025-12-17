using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Jumpscare")]
    public float jumpscareDistance = 0.5f;
    public float jumpscareSpeed = 10f;
    public AudioSource audioSource;
    public AudioClip jumpscareSound;

    private bool active;
    private Transform enemy;

    void Awake()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (!active || enemy == null) return;

        Vector3 targetPos =
            enemy.position +
            enemy.forward * jumpscareDistance +
            Vector3.up * 0.15f;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            jumpscareSpeed * Time.unscaledDeltaTime
        );

        Quaternion rot = Quaternion.LookRotation(enemy.position - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            jumpscareSpeed * Time.unscaledDeltaTime
        );
    }

    public void TriggerJumpscare(Transform enemyTransform)
    {
        if (active) return;

        active = true;
        enemy = enemyTransform;

        if (audioSource && jumpscareSound)
            audioSource.PlayOneShot(jumpscareSound);

        Time.timeScale = 0f;
    }
}
