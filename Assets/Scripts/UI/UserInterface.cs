using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI weaponInfoField;
    [SerializeField] private TextMeshProUGUI levelInfoField;
    [SerializeField] private TextMeshProUGUI playerInfoField;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAP(CombatCharacter character) => actionPointsText.text = character.PlanningAP+" AP";

    public void ChangeWeapon()
    {
        if (Status.Current == "planning")
        {
            CombatCharacter.cCList[Status.Player].usesOffHand = !CombatCharacter.cCList[Status.Player].usesOffHand;
            ShowWeaponStats();
        }
    }


    public void ShowWeaponStats()
    {
        if (!CombatCharacter.cCList[Status.Player].usesOffHand)
        {
            ShowWeaponStats(CombatCharacter.cCList[Status.Player].equipment[0]);
        }
        else
        {
            ShowWeaponStats(CombatCharacter.cCList[Status.Player].equipment[1]);
        }
    }
    public void ShowWeaponStats(Item weapon)
    {
        string weaponText = $"{weapon.itemName} [ {weapon.apCost} AP ]\n";
        weaponText += "Damage: " + weapon.DamageDiapason + " ";
        if (weapon.rangedAttack)
            weaponText += "Range: "+weapon.Range + "\n";
        else
            weaponText += "Melee\n";
        weaponText += "Skill: " + CombatCharacter.cCList[Status.Player].skills[weapon.skillname]+" %";
        weaponInfoField.text = weaponText;
    }

    public void RefreshLevelInfo()
    {
        int totalEnemiesLevel = 0;
        string levelInfoText = "";
        foreach (CombatCharacter cChar in CombatCharacter.cCList)
        {
            if (cChar.ai == "")
                levelInfoText += cChar.charName + " " + cChar.level + " lvl ";
            else if (!cChar.Dead)
            {
                totalEnemiesLevel += cChar.level;
            }
        }
        levelInfoText += "\nEnemyes "+totalEnemiesLevel+" total lvl";
        levelInfoField.text = levelInfoText;
    }

    public void RefreshCharInfo() => RefreshCharInfo(CombatCharacter.cCList[Status.Player]);
    public void RefreshCharInfo(CombatCharacter player)
    {
        string charInfoText = $"{player.charName} [ {player.level} lvl ]\n"
                            + player.ExperienceText+"\n\n"
                            + $"ST {player.ST} [+0 melee damage]\n"  //TODO Add melee damage
                            + $"PE {player.PE} [{player.PE-1} aim shoot range]\n" //TODO Change range formula to Property??
                            + $"EN {player.EN} [{player.MaxHP} Max HP]\n"
                            + $"AG {player.AG} [{player.totalAP} AP, {player.AC} AC]";
        playerInfoField.text = charInfoText;
    }

    public void EndTurn()
    {
        if (Status.Current != "planning")
            return;
        CombatCharacter activeCharacter = CombatCharacter.cCList[Status.Player];
        if (activeCharacter.PlanningAP > 0)
            CombatAction.Wait(activeCharacter, activeCharacter.PlanningAP);
        Status.NextPlayer();
    }
    public void Wait()
    {
        if (Status.Current != "planning")
            return;
        CombatAction.Wait(CombatCharacter.cCList[Status.Player]);
        if (CombatCharacter.cCList[Status.Player].PlanningAP == 0)
        {
            Status.NextPlayer();
        }
    }

    public void ShowBigMessage (string message)
    {
        print(message);
    }
}
