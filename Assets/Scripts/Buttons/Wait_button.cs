using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait_button : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        CombatAction.Wait(CombatCharacter.cCList[Status.player]);
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
