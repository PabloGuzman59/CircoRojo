using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int activatedGenerators = 0;

    public MonsterAI monster;
    public ExitDoor exitDoor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // NUEVO: Iniciar en fase 0 (modo pasivo)
        if (monster != null)
        {
            monster.SetPhase(0);
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER: Monster no asignado en el inspector.");
        }
    }

    public void OnGeneratorActivated(int id)
    {
        activatedGenerators++;
        Debug.Log("GAMEMANAGER: Generadores activados = " + activatedGenerators);

        if (activatedGenerators == 1)
        {
            if (monster != null)
                monster.SetPhase(1);
            else
                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");
        }
        else if (activatedGenerators == 2)
        {
            if (monster != null)
                monster.SetPhase(2);
            else
                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");
        }
        else if (activatedGenerators == 3)
        {
            if (monster != null)
                monster.SetPhase(3);
            else
                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");

            if (exitDoor != null)
            {
                exitDoor.Unlock();
            }
            else
            {
                Debug.LogWarning("GAMEMANAGER: exitDoor no asignado en el inspector.");
            }
        }
    }

    public void PlayerDied()
    {
        Debug.Log("GAME MANAGER: Se registró la muerte del jugador.");
        Debug.Log("GAME OVER: Aquí más adelante activaremos la UI de derrota.");

        // Más adelante:
        // UIManager.Instance.ShowDeathScreen();
        // Stop time, fade, etc.
    }

    public void PlayerEscaped()
    {
        Debug.Log("GAMEMANAGER: El jugador ha escapado. VICTORIA.");
        // Aquí luego: UI de victoria, cambiar de escena, etc.
    }
}
