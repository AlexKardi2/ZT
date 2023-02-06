using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickArea : MonoBehaviour
{
    public string action = "move";
    
    public int xCorrection;
    public int yCorrection;
    public CombatCharacter combatCharacter; //Subject for MOVE & target (object) for ATTACK
    public int costOD; //for MOVE only

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
            combatCharacter.OverheadText.Show("To hit: __%");
        }
    }

    private void OnMouseExit()
    {
        combatCharacter.OverheadText.ShowHP();
    }

    
    private void OnMouseDown()
    {
        //Start move action
        if (action=="move")
        {
            if (combatCharacter.planningOD >= costOD)
            {
                xPlace = combatCharacter.planningPos[0] + xCorrection;
                yPlace = combatCharacter.planningPos[1] + yCorrection;
                combatCharacter.MovePlan(xPlace, yPlace);
            }
        } else if (action== "attack")
        {
            //Check for and perform move action
            if (Input.GetKey(KeyCode.LeftControl) == true || Input.GetKey(KeyCode.RightControl) == true)
            {
                CombatCharacter planningCharacter = CombatCharacter.cCList[Status.player];
                xPlace = combatCharacter.planningPos[0];
                yPlace = combatCharacter.planningPos[1];
                planningCharacter.MovePlan(xPlace, yPlace);

            } else
            {
                
                if (CombatAction.Attack(CombatCharacter.cCList[Status.player], combatCharacter))
                {
                    //print("ATTACKING " + combatCharacter.name);
                   
                }

            }
            
        }

        if (CombatCharacter.cCList[Status.player].planningOD == 0)
        {
            Status.NextPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
