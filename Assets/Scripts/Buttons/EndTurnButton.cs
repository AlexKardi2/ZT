using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        CombatCharacter activeCharacter = CombatCharacter.cCList[Status.player];
        if (activeCharacter.planningOD>0)
            CombatAction.Wait(activeCharacter,activeCharacter.planningOD);
        Status.NextPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
