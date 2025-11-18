using UnityEngine;

public class FlashlightNormal : MonoBehaviour
{
    public Light lightObj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lightObj.enabled = !lightObj.enabled;
        }
    }
}
