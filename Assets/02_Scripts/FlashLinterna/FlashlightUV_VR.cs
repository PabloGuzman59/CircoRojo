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

    // --- NUEVO ---
    private MinionAI lastMinion;
    private MonsterAI lastMonster;

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
                {
                    min.ApplyUV(true);   // <<< CORREGIDO
                    lastMinion = min;
                }
                else if (lastMinion != null)
                {
                    lastMinion.ApplyUV(false);
                    lastMinion = null;
                }

                var monster = hit.collider.GetComponentInParent<MonsterAI>();
                if (monster != null)
                {
                    monster.ApplyUV(true);  // <<< CORREGIDO
                    lastMonster = monster;
                }
                else if (lastMonster != null)
                {
                    lastMonster.ApplyUV(false);
                    lastMonster = null;
                }
            }
        }
        else
        {
            uvLight.enabled = false;
            charge += recharge * Time.deltaTime;

            // Si sueltas el botón o se apaga la linterna,
            // detén el efecto UV en el minion o monstruo
            if (lastMinion != null)
            {
                lastMinion.ApplyUV(false);
                lastMinion = null;
            }

            if (lastMonster != null)
            {
                //lastMonster.ApplyUV(false);
                lastMonster = null;
            }
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
