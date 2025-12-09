using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightUV_VR : MonoBehaviour
{
    public Light uvLight;

    public float maxCharge = 100f;
    public float charge = 100f;
    public float drain = 15f;
    public float recharge = 5f;

    public PlayerInventory inv;

    public Transform handDirection; // mano derecha o ancla del láser
    public float dist = 10f;

    public InputActionReference uvButton; // Acción del XR Controller

    void Update()
    {
        bool isPressed = uvButton.action.IsPressed();

        if (isPressed && charge > 0)
        {
            uvLight.enabled = true;
            charge -= drain * Time.deltaTime;

            if (Physics.Raycast(handDirection.position, handDirection.forward, out RaycastHit hit, dist))
            {
                var min = hit.collider.GetComponentInParent<MinionAI>();
                if (min != null)
                    min.ApplyUV();

                var monster = hit.collider.GetComponentInParent<MonsterAI>();
                if (monster != null)
                    monster.ApplyUV();
            }
        }
        else
        {
            uvLight.enabled = false;
            charge += recharge * Time.deltaTime;
        }

        charge = Mathf.Clamp(charge, 0, maxCharge);
    }

    public void Recharge()
    {
        if (inv.batteryCount > 0)
        {
            inv.batteryCount--;
            charge += 40f;
        }
    }
}
