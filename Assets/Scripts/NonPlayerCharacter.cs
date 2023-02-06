using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : CombatCharacter
{
    //Variables for NPC


    // Start is called before the first frame update
    private void Start()
    {
        //Getting components to technical use variables
        OverheadText = GetComponentInChildren<OverheadMessage>();

        ResetOD();
        HP = maxHP;

        //TEMP setting places for Combatcharacters
        int i = cCList.IndexOf(this);
        CombatCharacter.cCList[i].pos[0] = i;
        CombatCharacter.cCList[i].pos[1] = i;

        //This is not temporary part
        ResetPlanning();
        CombatCharacter.cCList[i].transform.position = new Vector3(CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 0], CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 1], 0);

        //temp creating attackzone
        attackZone = Instantiate<GameObject>(prefabClickZone);
        if (attackZone != null)
            print("AZone for " + name + " is created");
        else
            print("Error!!! AZone for " + name + "isn't created");
        attackZone.transform.parent = this.transform;
        attackZone.transform.position = new Vector3(CoordArray.cArray[this.pos[0], this.pos[1], 0], CoordArray.cArray[this.pos[0], this.pos[1], 1], 0);
        attackZone.GetComponent<ClickArea>().combatCharacter = this;
        attackZone.GetComponent<ClickArea>().action = "attack";
        attackZone.SetActive(false);

        //ADD when needed if (CombatCharacter.cCList[i].equipment[0]==null) CombatCharacter.cCList[i].equipment[0]=Item.items[0]; if (CombatCharacter.cCList[i].equipment[0]==null) CombatCharacter.cCList[i].equipment[1]=Item.items[0];

        //TEMP part continuing
        bool readyCheck = true;
        foreach (CombatCharacter checkingCharacter in cCList)
        {
            if (checkingCharacter.attackZone == null) readyCheck = false;
        }

        if (readyCheck)
        {
            print("Ready Check sucsessful for " + cCList.Count);
            CombatCharacter.cCList[Status.player].StartPlanning();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SpawnRat(int level = 1)
    {
        string ai = "rat";
        int maxHP = 10;
        int totalOD = 6;
        int AC = 5;
        
        bool rangedAttack = false;
        int damageMultipler = 1;
        int damageDise = 4;
        int damagePlus = 0;
        int damageRange = 1;
        int attackOD=3;

        GameObject npcGameObj = Instantiate(PrefabsList.instance.ratPrefab);
        NonPlayerCharacter npc = npcGameObj.GetComponent<NonPlayerCharacter>();
        npc.ai = ai;
        npc.maxHP = maxHP;
        npc.totalOD = totalOD;
        npc.AC = AC;

        Item attack = new Item();
        attack.range = damageRange;
        attack.odCost = attackOD;
        attack.rangedAttack = rangedAttack;
        attack.SetDamage(damageMultipler, damageDise, damagePlus);
        attack.skillname = "npcattack";
        npc.equipment.Add(attack);
        npc.equipment.Add(attack);


        if (!npc.skills.ContainsKey(attack.skillname))
            npc.skills.Add(attack.skillname, 0);
        npc.skills[attack.skillname] = 75;

        
        for (int i=1; i<level; i++)
        {
            npc.maxHP += maxHP/2;
            int randomStart = 0;
            if (npc.totalOD >= (totalOD * 2))
                randomStart = 1;
            switch (Random.Range(randomStart, 4))
            {
                case 0:
                    npc.totalOD++;
                    break;
                case 1:
                    attack.BoostDamage();
                    break;
                case 2:
                    attack.BoostDamage("dice");
                    break;
                case 3:
                    npc.AC++;
                    break;
            }
        }
    }

    public void DeathProtocol ()
    {
        
    }
}
