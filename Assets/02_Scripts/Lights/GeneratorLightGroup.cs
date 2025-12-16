using UnityEngine;

public class GeneratorLightGroup : MonoBehaviour
{
    [Header("Grupo de luces")]
    public Light[] lights;

    [Header("Opcional")]
    public bool startOff = true;

    void Start()
    {
        if (startOff)
            SetLights(false);
    }

    public void SetLights(bool state)
    {
        foreach (Light l in lights)
        {
            if (l != null)
                l.enabled = state;
        }
    }
}
