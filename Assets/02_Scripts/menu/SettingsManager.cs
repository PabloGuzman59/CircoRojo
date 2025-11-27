using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider brightnessSlider;
    public Slider musicSlider;

    [Header("Brillo")]
    public Image brightnessOverlay;
    // 0 = no oscurece, 1 = pantalla negra
    public float maxDarkness = 0.7f;

    [Header("Música")]
    public AudioSource musicSource;

    const string BrightnessKey = "brightness";
    const string MusicKey = "musicVolume";

    void Start()
    {
        // Cargar valores guardados o usar por defecto
        float b = PlayerPrefs.GetFloat(BrightnessKey, 0.7f); // 0.7 = medio claro
        float m = PlayerPrefs.GetFloat(MusicKey, 0.8f);      // 0.8 = alto pero no máximo

        if (brightnessSlider != null)
        {
            brightnessSlider.value = b;
            ApplyBrightness(b);
        }

        if (musicSlider != null)
        {
            musicSlider.value = m;
            ApplyMusicVolume(m);
        }
    }

    // Llamado por el slider de brillo
    public void OnBrightnessChanged(float value)
    {
        ApplyBrightness(value);
        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }

    // Llamado por el slider de música
    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(MusicKey, value);
        PlayerPrefs.Save();
    }

    void ApplyBrightness(float value)
    {
        if (brightnessOverlay == null) return;

        // value: 0 = muy oscuro, 1 = muy claro
        float alpha = (1f - Mathf.Clamp01(value)) * maxDarkness;
        Color c = brightnessOverlay.color;
        c.a = alpha;
        brightnessOverlay.color = c;
    }

    void ApplyMusicVolume(float value)
    {
        if (musicSource == null) return;

        // Clampeamos para evitar valores raros
        musicSource.volume = Mathf.Clamp01(value);
    }
}
