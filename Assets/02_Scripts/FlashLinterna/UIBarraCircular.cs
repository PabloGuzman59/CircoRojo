using UnityEngine;
using UnityEngine.UI;

public class UIBarraCircular : MonoBehaviour
{
    [Header("Referencias")]
    public Slider slider;   // SliderUV

    void Start()
    {
        // Auto-asignar si está vacío
        if (slider == null)
            slider = GetComponent<Slider>();

        // Configuración segura del slider
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        // Configurar el Fill automáticamente (solo una vez)
        Image fillImage = slider.fillRect.GetComponent<Image>();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Radial360;
        fillImage.fillOrigin = 0; // Top
    }

    public void ActualizarBarra(float valorActual, float valorMaximo)
    {
        if (slider == null || valorMaximo <= 0) return;

        slider.value = Mathf.Clamp01(valorActual / valorMaximo);
    }
}
