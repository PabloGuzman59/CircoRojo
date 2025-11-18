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

    public void OnGeneratorActivated(int id)
    {
        activatedGenerators++;
        Debug.Log("Generadores activados: " + activatedGenerators);

        if (activatedGenerators == 1)
        {
            monster.SetPhase(1);
        }
        else if (activatedGenerators == 2)
        {
            monster.SetPhase(2);
        }
        else if (activatedGenerators == 3)
        {
            monster.SetPhase(3);
            exitDoor.Unlock();
        }
    }
}
