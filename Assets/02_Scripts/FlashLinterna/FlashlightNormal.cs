using UnityEngine;

public class FlashlightNormal : MonoBehaviour
{
    public Light lightObj;

    public void TurnOff()
    {
        lightObj.enabled = false;
    }

    void Update()
    {
        // Si presiono F: toggle
        if (Input.GetKeyDown(KeyCode.F))
        {
            lightObj.enabled = !lightObj.enabled;
            Debug.Log("Linterna normal: " + (lightObj.enabled ? "ON" : "OFF"));
        }
    }
}
