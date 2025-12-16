using UnityEngine;

public class FlashlightClick_Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Light lightObj;
    private bool lastState;

    void Start()
    {
        // Busca la luz automáticamente
        lightObj = GetComponent<FlashlightNormal_VR>()?.lightObj;

        if (lightObj != null)
            lastState = lightObj.enabled;
    }

    void Update()
    {
        if (lightObj == null || audioSource == null || clickSound == null) return;

        bool currentState = lightObj.enabled;

        // Si cambió ON ↔ OFF
        if (currentState != lastState)
        {
            audioSource.PlayOneShot(clickSound);
            lastState = currentState;
        }
    }
}
