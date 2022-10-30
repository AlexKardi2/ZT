using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    //Combat logs
    public static List<CombatAction> planningList = new List<CombatAction>();
    public static List<CombatAction> combatLog = new List<CombatAction>();

    public List<CombatAction> showCombatLog = combatLog;

    public static string current = "planning";

    //Variables for "movie" status

    public static int playTurn = 0;
    public static int playAct = 0;

    //Variables for "planning" status
    public static int turn=0;
    public static int player = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void NextPlayer()
    {
        CombatCharacter.cCList[player].ResetPlanning();
        CombatCharacter.cCList[player].StartPlanning(false);
        player++;
        
        if (player<CombatCharacter.cCList.Count)
        {
            CombatCharacter.cCList[player].StartPlanning();
        } else
        {
            player = 0; //May be delete
            
            CombatAction.CreatePlanningList(planningList);

            CombatAction.Perform(planningList);

            current = "movie";
        }
    }

    public static void NextTurn()
    {
        print("TURN IS FINISHED. Starting turn "+(turn+1));
        turn++;
        current = "planning";

        foreach (CombatCharacter cC in CombatCharacter.cCList)
        {
            cC.ResetOD();
            cC.ResetPlanning();
        }

        player = 0;
        CombatCharacter.cCList[player].StartPlanning();
    }
}
