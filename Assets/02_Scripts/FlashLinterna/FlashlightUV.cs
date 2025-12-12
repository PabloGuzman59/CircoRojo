using UnityEngine;

public class FlashlightUV : MonoBehaviour
{
    public Light uvLight;

    public float maxCharge = 100f;
    public float charge = 100f;
    public float drain = 15f;
    public float recharge = 5f;

    public PlayerInventory inv;

    public Transform cam;
    public float dist = 10f;

    // NUEVO: referencia a la linterna normal
    public FlashlightNormal normalFlashlight;
    // ✅ AÑADE ESTA LÍNEA:
    public UIBarraCircular uiBarra;

    void Update()
    {
        // BOTÓN DERECHO (activar UV)
        if (Input.GetMouseButton(1))
        {
            if (charge > 0)
            {
                // APAGAR LA LINTERNA NORMAL
                if (normalFlashlight != null)
                    normalFlashlight.TurnOff();

                uvLight.enabled = true;
                charge -= drain * Time.deltaTime;

                // RAYCAST UV
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, dist))
                {
                    // MINION
                    var min = hit.collider.GetComponentInParent<MinionAI>();
                    if (min != null)
                    {
                        Debug.Log("UV HIT → MINION");
                        //min.ApplyUV();
                    }

                    // MONSTER
                    var monster = hit.collider.GetComponentInParent<MonsterAI>();
                    if (monster != null)
                    {
                        Debug.Log("UV HIT → MONSTER");
                        //monster.ApplyUV();
                    }
                }

            }
            else
            {
                uvLight.enabled = false;
            }
        }
        else
        {
            uvLight.enabled = false;
            charge += recharge * Time.deltaTime;
        }

        // RECARGAR CON R
        if (Input.GetKeyDown(KeyCode.R) && inv.batteryCount > 0)
        {
            inv.batteryCount--;
            charge += 40f;
            Debug.Log("Recargando UV. Pilas: " + inv.batteryCount);
        }

        charge = Mathf.Clamp(charge, 0, maxCharge);
        // Aquí se actualizaría la UI
        // ✅ AÑADE ESTAS 2 LÍNEAS:
        if (uiBarra != null)
            uiBarra.ActualizarBarra(charge, maxCharge);
    }
}
