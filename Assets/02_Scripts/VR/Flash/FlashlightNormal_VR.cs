using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightNormal_VR : MonoBehaviour
{
    public Light lightObj;
    public InputActionReference toggleButton;   // Botón "B" en Oculus o "Circle" en PS5

    void Update()
    {
        if (toggleButton.action.WasPressedThisFrame())
        {
            lightObj.enabled = !lightObj.enabled;
            Debug.Log("Linterna normal ? " + (lightObj.enabled ? "ON" : "OFF"));
        }
    }
}
