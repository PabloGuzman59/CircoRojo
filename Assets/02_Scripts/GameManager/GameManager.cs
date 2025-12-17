using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Generadores y Luces")]
    public int activatedGenerators = 0;
    public GeneratorLightGroup[] lightGroups;

    [Header("Monstruo y Puerta")]
    public MonsterAI monster;
    public ExitDoor exitDoor;

    [Header("Game Over y Victoria")]
    public GameOverManager gameOverManager;
    public VictoryManager victoryManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
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

        int index = id - 1;
        if (index >= 0 && index < lightGroups.Length)
        {
            lightGroups[index].SetLights(true);
        }

        if (activatedGenerators == 1)
        {
            if (monster != null) monster.SetPhase(1);
        }
        else if (activatedGenerators == 2)
        {
            if (monster != null) monster.SetPhase(2);
        }
        else if (activatedGenerators == 3)
        {
            if (monster != null) monster.SetPhase(3);

            if (exitDoor != null)
            {
                exitDoor.Unlock();
            }
        }
    }

    public void PlayerDied()
    {
        Debug.Log("GAME MANAGER: Se registró la muerte del jugador.");

        if (monster != null)
        {
            monster.enabled = false;
        }

        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
    }

    public void PlayerEscaped()
    {
        Debug.Log("GAMEMANAGER: El jugador ha escapado. VICTORIA.");

        if (monster != null)
        {
            monster.enabled = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerLook>().enabled = false;
        }

        if (victoryManager != null)
        {
            victoryManager.ShowVictory();
        }
    }
}
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance;

//    public int activatedGenerators = 0;

//    public MonsterAI monster;
//    public ExitDoor exitDoor;

//    public GeneratorLightGroup[] lightGroups;

//    void Awake()
//    {
//        instance = this;
//    }

//    void Start()
//    {
//        // NUEVO: Iniciar en fase 0 (modo pasivo)
//        if (monster != null)
//        {
//            monster.SetPhase(0);
//        }
//        else
//        {
//            Debug.LogWarning("GAMEMANAGER: Monster no asignado en el inspector.");
//        }
//    }

//    public void OnGeneratorActivated(int id)
//    {
//        activatedGenerators++;
//        Debug.Log("GAMEMANAGER: Generadores activados = " + activatedGenerators);

//        // 🔥 PRENDER GRUPO DE LUCES
//        int index = id - 1;
//        if (index >= 0 && index < lightGroups.Length)
//        {
//            lightGroups[index].SetLights(true);
//        }


//        if (activatedGenerators == 1)
//        {
//            if (monster != null)
//                monster.SetPhase(1);
//            else
//                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");
//        }
//        else if (activatedGenerators == 2)
//        {
//            if (monster != null)
//                monster.SetPhase(2);
//            else
//                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");
//        }
//        else if (activatedGenerators == 3)
//        {
//            if (monster != null)
//                monster.SetPhase(3);
//            else
//                Debug.LogWarning("GAMEMANAGER: Monster no asignado.");

//            if (exitDoor != null)
//            {
//                exitDoor.Unlock();
//            }
//            else
//            {
//                Debug.LogWarning("GAMEMANAGER: exitDoor no asignado en el inspector.");
//            }
//        }
//    }

//    public void PlayerDied()
//    {
//        Debug.Log("GAME MANAGER: Se registró la muerte del jugador.");
//        Debug.Log("GAME OVER: Aquí más adelante activaremos la UI de derrota.");

//        // Más adelante:
//        // UIManager.Instance.ShowDeathScreen();
//        // Stop time, fade, etc.
//    }

//    public void PlayerEscaped()
//    {
//        Debug.Log("GAMEMANAGER: El jugador ha escapado. VICTORIA.");
//        // Aquí luego: UI de victoria, cambiar de escena, etc.
//    }

//}
