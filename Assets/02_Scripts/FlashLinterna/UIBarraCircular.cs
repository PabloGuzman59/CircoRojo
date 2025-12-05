using UnityEngine;
using UnityEngine.UI;

public class UIBarraCircular : MonoBehaviour
{
    [Header("Referencias")]
    public Image fillImage; // Auto-asignará el Image del mismo GameObject

    void Start()
    {
        // Auto-asignar si está vacío
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Radial360;
            fillImage.fillOrigin = 0; // Top
            fillImage.fillAmount = 1f;
        }
    }

    public void ActualizarBarra(float valorActual, float valorMaximo)
    {
        if (fillImage == null) return;
        fillImage.fillAmount = valorActual / valorMaximo;
    
    }
}