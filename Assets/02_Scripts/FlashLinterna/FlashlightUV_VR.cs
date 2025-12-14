using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightUV_VR : MonoBehaviour
{
    [Header("Light")]
    public Light uvLight;

    [Header("Energy")]
    public float maxCharge = 100f;
    public float charge = 100f;
    public float drain = 15f;
    public float recharge = 5f;

    [Header("Inventory")]
    public PlayerInventory inv;

    [Header("Raycast")]
    public Transform handDirection; // Mano derecha o ancla del láser
    public float dist = 10f;

    [Header("Input")]
    public InputActionReference uvButton; // Acción XR (gatillo, grip, etc)

    // Referencias a enemigos impactados
    private MinionAI lastMinion;
    private MonsterAI lastMonster;

    void OnEnable()
    {
        if (uvButton != null)
            uvButton.action.Enable();
    }

    void OnDisable()
    {
        if (uvButton != null)
            uvButton.action.Disable();
    }

    void Update()
    {
        bool isPressed = uvButton != null && uvButton.action.IsPressed();

        if (isPressed && charge > 0)
        {
            uvLight.enabled = true;
            charge -= drain * Time.deltaTime;

            // DEBUG visual (opcional pero recomendado)
            Debug.DrawRay(handDirection.position, handDirection.forward * dist, Color.magenta);

            if (Physics.Raycast(handDirection.position, handDirection.forward, out RaycastHit hit, dist))
            {
                // --- MINION ---
                var min = hit.collider.GetComponentInParent<MinionAI>();
                if (min != null)
                {
                    min.ApplyUV(true);
                    lastMinion = min;
                }
                else if (lastMinion != null)
                {
                    lastMinion.ApplyUV(false);
                    lastMinion = null;
                }

                // --- MONSTER ---
                var monster = hit.collider.GetComponentInParent<MonsterAI>();
                if (monster != null)
                {
                    monster.ApplyUV(true);
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

            // Al soltar el botón, limpiar estados UV
            ClearUVEffects();
        }

        charge = Mathf.Clamp(charge, 0, maxCharge);
    }

    // Limpia cualquier efecto UV activo
    void ClearUVEffects()
    {
        if (lastMinion != null)
        {
            lastMinion.ApplyUV(false);
            lastMinion = null;
        }

        if (lastMonster != null)
        {
            lastMonster.ApplyUV(false);
            lastMonster = null;
        }
    }

    // Forzar apagado (para validaciones cruzadas)
    public void ForceOff()
    {
        uvLight.enabled = false;
        ClearUVEffects();
    }

    public void Set(bool state)
    {
        uvLight.enabled = state;
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
