using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        Item.LoadItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        foreach (CombatCharacter cChar in CombatCharacter.cCList) {
            Destroy(cChar.gameObject);
        }
        UserInterface.Instance.ShowBigMessage("GameOver");
        UserInterface.Instance.ShowMainMenu();
    }
    public void StartGame()
    {
        Status.FirstTurn();
        NonPlayerCharacter.SpawnRat(1);
        
        bool readyCheck = true;
        foreach (CombatCharacter checkingCharacter in CombatCharacter.cCList)
        {
            if (checkingCharacter.attackZone == null) readyCheck = false;
        }
        if (readyCheck)
        {
            CombatCharacter.cCList[Status.Player].StartPlanning();
        } else
        {
            print("ERROR!!! Something wrong with Starting game. Game stopped. Investigate this");
        }
    }
}
