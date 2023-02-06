using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    static private float rangeBalansingParametr = 1; //Multiplies max ranges of weapons & characters. Basicly 2 for FalloutPNP. 
    
    public string action;
    public CombatCharacter subject;
    public CombatCharacter target;
    //public CombatObject targetObject;
    public Item usedItem; 
    public int[] place = new int[2]; //[0] for X; [1] for Y
    public int oDCost;
    private int turn;
    
    public bool Move(CombatCharacter subj, int x, int y)
    {
        if (subj == null) return false;
        oDCost = Location.map[x, y].od;
        if (subj.SpendOD(oDCost, true))
        {
            action = "move";
            place[0] = x;
            place[1] = y;
            subject = subj;
            turn = Status.turn;
            return true;
        }
        else
        {
            print("PlanOD =" + subj.planningOD + "is less then costOD=" + oDCost);
            print("Combat action haven't added");
            return false;
        }
    }

    public static bool Attack (CombatCharacter subj, CombatCharacter obj)
    {
        if ((subj == null)||(obj==null)) 
            return false;
        Item weapon;
        if (subj.usesOffHand)
            weapon= subj.equipment[1];
        else
            weapon = subj.equipment[0];

        if (subj.SpendOD(weapon.odCost, true))
        {
            CombatAction thisAttack = new CombatAction();
            thisAttack.turn = Status.turn;
            thisAttack.action = "attack";
            thisAttack.subject = subj;
            thisAttack.target = obj;
            thisAttack.usedItem = weapon;
            thisAttack.oDCost = weapon.odCost;
            subj.personalPlanningList.Add(thisAttack);
            return true;
        }
        else
            return false;
        
    }

    public static bool Wait(CombatCharacter subj, int oDcost=2)
    {
        if ((subj == null) || (oDcost < 1))
            return false;

        if (subj.SpendOD(oDcost, true))
        {
            CombatAction thisAction = new CombatAction();
            thisAction.turn = Status.turn;
            thisAction.oDCost = oDcost;
            thisAction.action = "wait";
            thisAction.subject = subj;

            subj.personalPlanningList.Add(thisAction);
            return true;
        }
        else
            return false;
    }

    public static void CreatePlanningList(List<CombatAction> pList)
    {
        pList.Clear();

        int totalLists = CombatCharacter.cCList.Count;
        int emptyLists = 0;

        for (int i = 0; emptyLists < totalLists; i++)
        {
            emptyLists = 0;
            foreach (CombatCharacter cChar in CombatCharacter.cCList)
            {
                if (i < cChar.personalPlanningList.Count)
                {
                    pList.Add(cChar.personalPlanningList[i]);
                }
                else
                {
                    emptyLists++;
                }
            }
        }

        foreach (CombatCharacter cChar in CombatCharacter.cCList)
            cChar.personalPlanningList.Clear();
    }

    public static void Perform(List<CombatAction> pList)
    {
        List<CombatAction> log = Status.combatLog;

        if ((log.Count != 0)&&(log[(log.Count - 1)].turn >= Status.turn))
        {
            print("ERROR! Previous turn " + log[(log.Count - 1)].turn + ">= current turn " + Status.turn);
            return;
        }
        else
        {
            //ADD here adding Set action for every life combat character (or not)
            print("It's OK with battle log");
        }

        foreach (CombatAction cA in pList)
        {
            if (cA.subject.dead == true)
            {
                cA.action = "skip";
                continue;
            }


            if (cA.action == "move")
            {
                bool checkList = true;
                checkList = checkList && (!(Mathf.Abs(cA.subject.pos[0] - cA.place[0]) > 1) || (Mathf.Abs(cA.subject.pos[1] - cA.place[1]) > 1)); //Checking for close tyle

                //ADD cheching for unavailable place

                //Checking for tyle without characters]
                foreach (CombatCharacter cC in CombatCharacter.cCList)
                {
                    if ((cC.pos[0] == cA.place[0]) && (cC.pos[1] == cA.place[1]))
                    {
                        checkList = false;
                        print("There is another character on the way");
                        print("Moving to " + cA.place[0] + " " + cA.place[1] + " but " + cC.name + "is there");
                    }
                }
                checkList = checkList && cA.subject.SpendOD(cA.oDCost);

                if (checkList)
                {
                    log.Add(cA);
                    cA.subject.pos[0] = cA.place[0];
                    cA.subject.pos[1] = cA.place[1];
                }

            }
            else if (cA.action == "attack")
            {
                Item weapon = cA.usedItem;
                if ((cA.subject.equipment[0]!=weapon)&&(cA.subject.equipment[1] != weapon)) {
                    print("You haven't this weapon to use !!");
                    continue;
                }
                
                int range = 1;
                if (weapon.rangedAttack)
                {
                    int weaponRange = (int)(weapon.range * (1 / rangeBalansingParametr));
                    int characterRange = (int)(cA.subject.PE * rangeBalansingParametr) - 1;
                    range = Mathf.Max(weaponRange, characterRange);
                }

                if (cA.target == null)
                {
                    //Find target from coordinates place[] and set a CombatCharacter or Object as target
                }
               
                bool checkList = true;

                if (range < Scripts.FindDistance(cA.subject.pos, cA.target.pos))
                    checkList=false;

                //ADD check for obstacle, change target to object if u need
                
                if (!checkList)
                    continue;

                int oDcost = Mathf.Max(cA.oDCost, weapon.odCost);

                if (checkList && cA.subject.SpendOD(oDcost))
                {
                    log.Add(cA);

                    int hitChanse = cA.subject.skills[weapon.skillname];
                    // ADD if (cA.target.ai != "") cA.target.CheckAC();
                    hitChanse -= cA.target.AC;
                    hitChanse -= cA.target.bonusAC;

                    if (Random.Range(0, 100) < hitChanse)
                    {
                        int damage = weapon.damage;
                        cA.target.HP -= damage;
                        print(cA.target.name + "'s got " + damage + " damage. Hit chance was " + hitChanse);
                        if (cA.target.HP <= 0)
                        {
                            //cA.target.HP=0;
                            cA.target.dead = true;
                            if (cA.target.ai!="")
                            {
                                NonPlayerCharacter npcTarget = (NonPlayerCharacter)cA.target;
                                npcTarget.DeathProtocol();
                                cA.subject.GetExperience(npcTarget);
                            }
                            //Add NPC death protocol ))
                            //Add Game over protocol ))

                        }
                    }
                    else
                    {
                        print(cA.subject.name + " have missed ((( HitChanse was " + hitChanse);
                        if (cA.subject.ai=="")
                            cA.subject.BoostSkill(weapon.skillname);
                    }
                }
            }
            else if (cA.action == "wait")
            {
                if (cA.subject.SpendOD(cA.oDCost))
                {
                    cA.subject.bonusAC += cA.oDCost;
                    log.Add(cA);

                    
                }
                else
                    print(cA.subject.name + " can't wait anymore *(");
            }
        }
    }
}
