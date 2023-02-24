using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        
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
    }
    public void StartGame()
    {
        
    }
}
