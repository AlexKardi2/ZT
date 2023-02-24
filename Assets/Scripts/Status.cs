using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    //Combat logs
    public static List<CombatAction> planningList = new List<CombatAction>();
    public static List<CombatAction> combatLog = new List<CombatAction>();

    public List<CombatAction> showCombatLog = combatLog;

    public static string Current { get; private set; } = "planning"; //TODO change to "starting"

    //Variables for "movie" status
    public static int MovieAct { get; private set; } = 0;

    //Variables for "planning" status
    public static int Turn { get; private set; } = 0;
    public static int Player { get; private set; } = 0;

    //Technical use variables
    private static GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public static void NextMovieAct ()
    {
        MovieAct++;
        if (MovieAct >= combatLog.Count)
        {
            Status.NextTurn();
        }
    }
    public static void NextPlayer()
    {
        CombatCharacter.cCList[Player].ResetPlanning();
        CombatCharacter.cCList[Player].StartPlanning(false);
        Player++;
        
        if (Player<CombatCharacter.cCList.Count)
        {
            CombatCharacter.cCList[Player].StartPlanning();
        } else
        {
            Player--;
            Current = "performing";

            CombatAction.CreatePlanningList(planningList);

            CombatAction.Perform(planningList);

            Current = "movie";
        }
    }

    private static void NextTurn()
    {
        print("TURN IS FINISHED. Starting turn "+(Turn+1));

        //Cleaning map and spawning 
        int totalEnemiesLevel=0;
        int totalPlayersLevel=0;
        
        int deadSouls = CleanDeadBodies();
        if (totalPlayersLevel == 0)
        {
            Current = "gameover";
            gameManager.GameOver();
            return;
        }

        SpawnEnemies(deadSouls);
        UserInterface.Instance.RefreshLevelInfo();
        Turn++;

        foreach (CombatCharacter cC in CombatCharacter.cCList)
        {
            cC.ResetAP();
            cC.ResetPlanning();
        }

        Player = 0;
        Current = "planning";
        CombatCharacter.cCList[Player].StartPlanning();

        void SpawnEnemies(int minSpawnNumber)
        {
            if (minSpawnNumber < 1)
                return;
            int maxSpawnLevel = (totalPlayersLevel - totalEnemiesLevel) / minSpawnNumber;

            if (totalEnemiesLevel>1)
            {
                float recuceSpawnChanse = 1.0f / maxSpawnLevel;
                if (Random.value < recuceSpawnChanse)
                    minSpawnNumber--;
            }

            for (int i = 0; i < minSpawnNumber; i++)
            {
                int npcSpawnLevel = Mathf.Min(Random.Range(1, (totalPlayersLevel - totalEnemiesLevel + 1)), maxSpawnLevel);
                totalEnemiesLevel += npcSpawnLevel;
                NonPlayerCharacter.SpawnRat(npcSpawnLevel);
            }

            if (totalEnemiesLevel < totalPlayersLevel)
            {
                float additionalSpawnChanse = 1.0f - ((float)totalEnemiesLevel / totalPlayersLevel);
                float spawnRandom = Random.value;
                if (spawnRandom < additionalSpawnChanse)
                {
                    int npcSpawnLevel = Mathf.Min(Random.Range(1, (totalPlayersLevel - totalEnemiesLevel + 1)), maxSpawnLevel);
                    NonPlayerCharacter.SpawnRat(npcSpawnLevel);
                    print($"Was spawned additional NPC. Chanse was {additionalSpawnChanse}, random rolled {spawnRandom}");
                }
                else
                    print($"Additional NPC haven't spawned. Chanse was {additionalSpawnChanse}, random rolled {spawnRandom}");
            }

        }
        int CleanDeadBodies()
        {
            int cleanedBodies = 0;
            foreach (CombatCharacter cChar in CombatCharacter.cCList)
            {
                if (cChar.ai == "" && !cChar.Dead)
                    totalPlayersLevel += cChar.level;
                else
                {
                    if (cChar.Dead)
                    {
                        Destroy(cChar.transform.gameObject);
                        cleanedBodies++;
                    }
                    else
                        totalEnemiesLevel += cChar.level;
                }
            }
            return cleanedBodies;
        }
    }
}

