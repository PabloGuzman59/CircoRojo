using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightNormal_VR : MonoBehaviour
{
    [Header("Light")]
    public Light lightObj;

    [Header("Input")]
    public InputActionReference toggleButton;   // Primary Button (A)

    private bool wasPressedLastFrame = false;

    void OnEnable()
    {
        if (toggleButton != null)
            toggleButton.action.Enable();
    }

    void OnDisable()
    {
        if (toggleButton != null)
            toggleButton.action.Disable();
    }

    void Update()
    {
        bool isPressed = toggleButton != null && toggleButton.action.ReadValue<float>() > 0.5f;

        // Detectar SOLO cuando pasa de no presionado → presionado
        if (isPressed && !wasPressedLastFrame)
        {
            lightObj.enabled = !lightObj.enabled;
            Debug.Log("Linterna normal (A): " + (lightObj.enabled ? "ON" : "OFF"));
        }

        wasPressedLastFrame = isPressed;
    }

    public void Set(bool state)
    {
        lightObj.enabled = state;
    }
}
