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

                // RAYCAST
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, dist))
                {
                    var min = hit.collider.GetComponent<MinionAI>();
                    if (min != null) min.ApplyUV();

                    var monster = hit.collider.GetComponent<MonsterAI>();
                    if (monster != null)
                    {
                        monster.ApplyUV();
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
    }
}
