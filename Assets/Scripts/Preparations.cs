using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preparations : MonoBehaviour
{
    public List<CombatAction> combatLog = new List<CombatAction>();



    void Awake()
    {
        Item.LoadItems();
    }

    void Start()
    {
        /*Setting up starting places & othre stats for chars
        CombatCharacter.cCList[0].pos[0]=1;
        CombatCharacter.cCList[0].pos[1] =1;
        CombatCharacter.cCList[1].pos[0] = 5;
        CombatCharacter.cCList[1].pos[1] = 5;

        

        CombatCharacter.cCList[0].ResetPlanning();
        CombatCharacter.cCList[1].ResetPlanning();

        CombatCharacter.cCList[0].transform.position = new Vector3(CoordArray.cArray[CombatCharacter.cCList[0].pos[0], CombatCharacter.cCList[0].pos[1], 0], CoordArray.cArray[CombatCharacter.cCList[0].pos[0], CombatCharacter.cCList[0].pos[1], 1], 0);
        CombatCharacter.cCList[1].transform.position = new Vector3(CoordArray.cArray[CombatCharacter.cCList[1].pos[0], CombatCharacter.cCList[1].pos[1], 0], CoordArray.cArray[CombatCharacter.cCList[1].pos[0], CombatCharacter.cCList[1].pos[1], 1], 0);
        
        CombatCharacter.cCList[0].CreateClickZones();
        CombatCharacter.cCList[1].CreateClickZones();


        CombatCharacter.cCList[1].ai = "rat";
        CombatCharacter.cCList[Status.player].StartPlanning();

        Fulfilling CombatLog
        bool check;
        combatLog.Add(new CombatAction());
        check = combatLog[(combatLog.Count - 1)].Move(CombatCharacter.cCList[0], 1, 2);
        if (!check) combatLog.RemoveAt(combatLog.Count - 1);
        combatLog.Add(new CombatAction());`
        check = combatLog[(combatLog.Count - 1)].Move(CombatCharacter.cCList[1], 3, 2);
        if (!check) combatLog.RemoveAt(combatLog.Count - 1);
        combatLog.Add(new CombatAction());
        check = combatLog[(combatLog.Count - 1)].Move(CombatCharacter.cCList[1], 2, 2);
        if (!check) combatLog.RemoveAt(combatLog.Count - 1);*/

        //print("x" + CoordArray.cArray[7, 2, 0] + " y" + CoordArray.cArray[7, 2, 1]);

        //NonPlayerCharacter.SpawnRat(1);
    }

    // Update is called once per frame

}
