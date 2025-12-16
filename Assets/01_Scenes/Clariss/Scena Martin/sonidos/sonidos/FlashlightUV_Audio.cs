using UnityEngine;

public class FlashlightUV_Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip uvHum;

    private Light uvLight;
    private bool wasOn;

    void Start()
    {
        // Tomamos la luz UV directamente del script existente
        uvLight = GetComponent<FlashlightUV_VR>()?.uvLight;

        if (uvLight != null)
            wasOn = uvLight.enabled;
    }

    void Update()
    {
        if (uvLight == null || audioSource == null || uvHum == null)
            return;

        bool isOn = uvLight.enabled;

        // Cuando se enciende
        if (isOn && !wasOn)
        {
            audioSource.clip = uvHum;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Cuando se apaga
        if (!isOn && wasOn)
        {
            audioSource.Stop();
        }

        wasOn = isOn;
    }
}
