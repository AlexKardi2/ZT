using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickArea : MonoBehaviour
{
    public string action = "move";
    
    public int xCorrection;
    public int yCorrection;
    public CombatCharacter combatCharacter; //Subject for MOVE & target (object) for ATTACK
    public int costAP; //for MOVE only

    private int xPlace;
    private int yPlace;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnMouseEnter()
    {
        if (action=="attack")
        {
            combatCharacter.OverheadText.Show("To hit: "+ Scripts.HitChanse(CombatCharacter.cCList[Status.Player], combatCharacter)+"%");
        }
    }

    private void OnMouseExit()
    {
        combatCharacter.OverheadText.ShowHP();
    }


    private void OnMouseDown()
    {
        if (Status.Current != "planning")
            return;
        
        //Start move action
        if (action == "move")
        {
            if (combatCharacter.PlanningAP >= costAP)
            {
                xPlace = combatCharacter.planningPos[0] + xCorrection;
                yPlace = combatCharacter.planningPos[1] + yCorrection;
                combatCharacter.MovePlan(xPlace, yPlace);
            }
        }
        else if (action == "attack")
        {
            //Check for and perform move action
            if (Input.GetKey(KeyCode.LeftControl) == true || Input.GetKey(KeyCode.RightControl) == true)
            {
                CombatCharacter planningCharacter = CombatCharacter.cCList[Status.Player];
                xPlace = combatCharacter.planningPos[0];
                yPlace = combatCharacter.planningPos[1];
                planningCharacter.MovePlan(xPlace, yPlace);

            }
            else
            {
                if (CombatAction.Attack(CombatCharacter.cCList[Status.Player], combatCharacter))
                {
                    combatCharacter.OverheadText.ShowHP();
                }
            }
        }

        if (CombatCharacter.cCList[Status.Player].PlanningAP == 0)
        {
            Status.NextPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
