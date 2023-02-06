using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour
{
    //List of all Combat Characters in the scenes
    public static List<CombatCharacter> cCList = new();

    //Balansing variables
    private int levelUpMultipler = 2;

    //Technical use variables
    //setting X & Y corrections for 1st circle + List of child GameObjects
    private int[] xOddCorrArray = new int[] { 1, 1, 1, 0, -1, 0 }; //for Odd row
    private int[] xEvenCorrArray = new int[] { 0, 1, 0, -1, -1, -1 }; //for Even row
    private int[] yCorrArray = new int[] { 1, 0, -1, -1, 0, 1 };
    public GameObject prefabClickZone;
    public GameObject attackZone;
    public OverheadMessage OverheadText { get; protected set; }
    private List<GameObject> clickZones = new List<GameObject>();


    //Variables
    public string charName;
    public bool dead = false;
    public int level = 1;
    public int[] pos = new int[2];
    private int experience = 0;
    public bool usesOffHand = false;

    //[0] for right hand, [1] for left hand
    public List<Item> equipment = new();
    public Dictionary<string,int> skills = new();

    //Basic stats
    public int ST = 6; //CHANGE to 5 later //Strenght
    public int PE = 3; //CHANGE to 5 later //Perception
    public int EN = 6; //CHANGE to 5 later //Endurance
    public int AG = 8; //CHANGE to 5 later //Agility

    //Secondary stats vatiables
    public int totalOD; //Action points (ochki deystvija)
    private int OD;
    public int maxHP;
    public int AC; //Acmor class
    public int MD; //Melee damage
    public int bonusAC;
    private int _hp;

    //Secondary stats properties
    public int HP { get => _hp; set
        {
            if (_hp==0)
            {
                _hp = value;
            }
            else if (_hp>value)
            {
                OverheadText?.ShowRed((value - _hp) + " HP");
                _hp = value;
            } else
                _hp = value;
            OverheadText?.ShowHP();

        } }
 
    //AI Stats
    public string ai = "";

    //Planning stuff
    public int planningOD = 8;
    public int[] planningPos = new int[2];
    public List<CombatAction> personalPlanningList = new List<CombatAction>();



    //TEMP variables before have created item system


    // Start is called before the first frame update
    void Awake()
    {
        //print("Addint to cClist " + name);
        cCList.Add(this);
    }

    private void Start()
    {
        //Getting components to technical use variables
        OverheadText = GetComponentInChildren<OverheadMessage>();

        CalculateSecStats();
        ResetOD();
        HP = maxHP;

        //TEMP setting places for Combatcharacters
        int i = cCList.IndexOf(this);
        CombatCharacter.cCList[i].pos[0] = i;
        CombatCharacter.cCList[i].pos[1] = i;
        CombatCharacter.cCList[i].equipment.Add(null);
        CombatCharacter.cCList[i].equipment.Add(null);
        CombatCharacter.cCList[i].equipment[0] = Item.items[1];
        CombatCharacter.cCList[i].equipment[1] = Item.items[2];

        //This is not temporary part
        ResetPlanning();
        CombatCharacter.cCList[i].transform.position = new Vector3(CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 0], CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 1], 0);
        CombatCharacter.cCList[i].CreateClickZones();
        //ADD when needed if (CombatCharacter.cCList[i].equipment[0]==null) CombatCharacter.cCList[i].equipment[0]=Item.items[0]; if (CombatCharacter.cCList[i].equipment[0]==null) CombatCharacter.cCList[i].equipment[1]=Item.items[0];

        //TEMP part continuing
        bool readyCheck=true;
        //print("Ready Check started for "+cCList.Count);
        foreach (CombatCharacter checkingCharacter in cCList) {
            if (checkingCharacter.attackZone == null) readyCheck = false;
        }
        
        if (readyCheck)
        {
            //print("Ready Check sucsessful for "+cCList[i].name);   
            CombatCharacter.cCList[Status.player].StartPlanning();
        }
            

    }

    private void CalculateSecStats()
    {
        if (ST == 0) return; //Check for NPC
        totalOD = (AG / 2) + 5;
        maxHP = 15 + (ST + (2 * EN));
        AC = AG; //ADD here adding AC for armor

        //Calculating skills
        if (!skills.ContainsKey("melee")) 
            skills.Add("melee", 1);
        if (!skills.ContainsKey("guns"))
            skills.Add("guns", 1);
        if (!skills.ContainsKey("unarmed"))
            skills.Add("unarmed", 1);

        Dictionary <string,int> minimalSkills = new();

        minimalSkills.Add("melee", 20 + (2 * (ST + AG)));
        minimalSkills.Add("guns", 5 + 4 * AG);
        minimalSkills.Add("unarmed", 30 + (2 * (ST + AG)));

        foreach (KeyValuePair<string,int> mimimalSkill in minimalSkills)
        {
            skills[mimimalSkill.Key] = Mathf.Max(skills[mimimalSkill.Key], mimimalSkill.Value);
        }
    }

    public void CreateClickZones()
    {
        //Create correct x+y corrections arrays
        int[] xCorrArray = new int[yCorrArray.Length];
        if ((pos[1] % 2) == 0)
            xCorrArray = xEvenCorrArray;
        else
            xCorrArray = xOddCorrArray;

        //Create array for corrections coordinates of ClickZones
        List<float[]> clickzoneCoordCorrections = new List<float[]>();
        for (int i = 0; i < yCorrArray.Length; i++)
        {
            int x = 1 + xOddCorrArray[i];
            int y = 1 + yCorrArray[i];
            float[] thisCoords = new float[2];

            thisCoords[0] = (CoordArray.cArray[x, y, 0] - CoordArray.cArray[1, 1, 0]);
            thisCoords[1] = (CoordArray.cArray[x, y, 1] - CoordArray.cArray[1, 1, 1]);

            clickzoneCoordCorrections.Add(thisCoords);
        }

        for (int i = 0; i < yCorrArray.Length; i++)
        {
            clickZones.Add(Instantiate<GameObject>(prefabClickZone));
            clickZones[i].transform.parent = this.transform;
            clickZones[i].GetComponent<ClickArea>().combatCharacter = this;

            //Setting X & Y corrections for zones
            clickZones[i].GetComponent<ClickArea>().xCorrection = xCorrArray[i];
            clickZones[i].GetComponent<ClickArea>().yCorrection = yCorrArray[i];

            //Setting places for click zones
            //clickZones[i].transform.position = new Vector3 (CoordArray.cArray[(this.pos[0] + xCorrArray[i]), (this.pos[1] + yCorrArray[i]), 0], CoordArray.cArray[(this.pos[0] + xCorrArray[i]), (this.pos[1] + yCorrArray[i]), 1], 0);
            clickZones[i].transform.position = new Vector3((this.transform.position.x + clickzoneCoordCorrections[i][0]), (this.transform.position.y + clickzoneCoordCorrections[i][1]), 0);

            clickZones[i].SetActive(false); //Turning them off
        }

        attackZone = Instantiate<GameObject>(prefabClickZone);
        /*if (attackZone != null)
            print("AZone for " + name + " is created");
        else
            print("Error!!! AZone for " + name + "isn't created");*/
        attackZone.transform.parent = this.transform;
        attackZone.transform.position = new Vector3(CoordArray.cArray[this.pos[0], this.pos[1], 0], CoordArray.cArray[this.pos[0], this.pos[1], 1], 0);
        attackZone.GetComponent<ClickArea>().combatCharacter = this;
        attackZone.GetComponent<ClickArea>().action = "attack";
        attackZone.SetActive(false);
    }

    public void ResetPlanning()
    {
        planningOD = totalOD;
        planningPos[0] = pos[0];
        planningPos[1] = pos[1];
        this.transform.position = new Vector3(CoordArray.cArray[this.pos[0], this.pos[1], 0], CoordArray.cArray[this.pos[0], this.pos[1], 1], 0);
    }

    public void StartPlanning(bool start = true)
    {
        if (start && this.ai != "")
        {
            if (!dead)
            {
                print("I want to start AI");
                Scripts.Ai(this);
                print("AI have finished work");
            }
            Status.NextPlayer();
            return;
        }

        for (int i = 0; i < clickZones.Count; i++)
        {

            if ((planningPos[1] % 2) == 0)
                clickZones[i].GetComponent<ClickArea>().xCorrection = xEvenCorrArray[i];
            else
                clickZones[i].GetComponent<ClickArea>().xCorrection = xOddCorrArray[i];
        }

        foreach (GameObject cz in clickZones)
        {
            if (start)
            {
                int xCurrent = pos[0] + cz.GetComponent<ClickArea>().xCorrection;
                int yCurrent = pos[1] + cz.GetComponent<ClickArea>().yCorrection;

                bool check = true;
                check = (xCurrent >= 0) && (xCurrent < Location.xSize) && (yCurrent >= 0) && (yCurrent < Location.ySize);
                foreach (CombatCharacter cC in CombatCharacter.cCList)
                {
                    if ((xCurrent == cC.planningPos[0]) && (yCurrent == cC.planningPos[1]))
                        check = false;
                }
                if (check)
                {
                    cz.SetActive(true);
                    cz.GetComponent<ClickArea>().costOD = Location.map[xCurrent, yCurrent].od;
                }
            }
            else
                cz.SetActive(false);
        }

        foreach (CombatCharacter cC in CombatCharacter.cCList)
        {
            cC.attackZone.SetActive(start);
            if (cC == this)
            {
                cC.attackZone.SetActive(false);
            }
        }

    }

    public void GetExperience(NonPlayerCharacter killedNPC)
    {
        experience += killedNPC.level;
        if (experience>=level*levelUpMultipler)
        {
            //LevelUp protocol
            experience -= level * levelUpMultipler;
            level++;

            int randomStart = 0;
            int statSum = ST + PE + EN + AG;
            if (statSum >= 40) randomStart++; 

            switch (Random.Range(randomStart, 4))
            {
                case 0:
                    //Improving stats
                    int randomResult = Random.Range(1, (statSum + 1));
                    if (randomResult<=ST)
                    {
                        if (ST < 10) ST++;
                        else randomResult = ST + 1;
                    }
                    if ((randomResult > ST) && (randomResult <= ST+PE))
                    {
                        if (PE < 10) PE++;
                        else randomResult = ST+PE+ 1;
                    }
                    if ((randomResult > ST+PE) && (randomResult <= ST+PE+EN))
                    {
                        if (EN < 10) EN++;
                        else randomResult = ST + PE  + EN+ 1;
                    }
                    if (randomResult> ST + PE + EN)
                    {
                        if (AG < 10) AG++;
                        else if (ST < 10) ST++;
                        else if (PE < 10) PE++;
                        else EN++;
                    }
                    CalculateSecStats();
                    break;
                case 1:
                    int randomResult1 = Random.Range(0, 10);
                    if (randomResult1==0)
                    {
                        equipment[0].BoostDamage("multipler");
                    } else if (randomResult1 < 5)
                    {
                        equipment[0].BoostDamage("dice", 3);
                    } else
                        equipment[0].BoostDamage();

                    break;
                case 2:
                    int randomResult2 = Random.Range(0, 10);
                    if (randomResult2 == 0)
                    {
                        equipment[0].BoostDamage("multipler");
                    }
                    else if (randomResult2 < 5)
                    {
                        equipment[0].BoostDamage("dice", 3);
                    }
                    else
                        equipment[0].BoostDamage();
                    break;
                /*case 3:    
                    equipment[3].BoostArmor();
                    break;*/
            }

        }
    }

    public void ResetOD()
    {
        OD = totalOD;
        bonusAC = 0;

    }

    public bool SpendOD(int cost, bool isPlanning = false)
    {
        if (isPlanning == false)
        {
            if (OD < cost)
                return false;
            else
            {
                OD -= cost;
                return true;
            }
        }
        else
        {
            if (planningOD < cost)
                return false;
            else
            {
                planningOD -= cost;
                return true;
            }
        }

    }

    public void MovePlan(int x, int y)
    {
        if ((Mathf.Abs(planningPos[0] - x) > 1) || (Mathf.Abs(planningPos[1] - y) > 1))
        {
            print("WRONG coordinates for MovePlan!!!");
            return;
        }

        int oDcost = Location.map[x, y].od;

        if ((planningOD - oDcost) < 0) return;

        //Creating planning combat action
        //print("Triying to add move action to " + x + " " + y);

        bool check;
        personalPlanningList.Add(new CombatAction());
        check = personalPlanningList[(personalPlanningList.Count - 1)].Move(this, x, y);
        if (!check)
        {
            personalPlanningList.RemoveAt(personalPlanningList.Count - 1);
            print("Haven't done this (");
        }
        else
        {
            //Moving plan position and sprite
            planningPos[0] = x;
            planningPos[1] = y;
            this.transform.position = new Vector3(CoordArray.cArray[x, y, 0], CoordArray.cArray[x, y, 1], 0);


            for (int i = 0; i < clickZones.Count; i++)
            {

                if ((planningPos[1] % 2) == 0)
                    clickZones[i].GetComponent<ClickArea>().xCorrection = xEvenCorrArray[i];
                else
                    clickZones[i].GetComponent<ClickArea>().xCorrection = xOddCorrArray[i];
            }

            //Are ClickZones exist or not (can we move somewhere or not)
            foreach (GameObject cz in clickZones)
            {
                int xCurrent = planningPos[0] + cz.GetComponent<ClickArea>().xCorrection;
                int yCurrent = planningPos[1] + cz.GetComponent<ClickArea>().yCorrection;

                check = (xCurrent >= 0) && (xCurrent < Location.xSize) && (yCurrent >= 0) && (yCurrent < Location.ySize);
                foreach (CombatCharacter cC in CombatCharacter.cCList)
                {
                    if ((xCurrent == cC.planningPos[0]) && (yCurrent == cC.planningPos[1]))
                        check = false;
                }

                if (check)
                {
                    cz.SetActive(true);
                    cz.GetComponent<ClickArea>().costOD = Location.map[xCurrent, yCurrent].od;
                }
                else
                    cz.SetActive(false);
            }
        }

        /*if (planningOD == 0) 
            Status.NextPlayer();*/

    }

    public void BoostSkill(string skillname)
    {
        float difficalty = 0.1f; //<1 - much easier to train; >1 - much more difficult; 0-always trains
        
        if ((skillname == "") || (!skills.ContainsKey(skillname)))
            return;

        if (skills[skillname] >= 300)
            return;

        float chanse = 1-(float)(skills[skillname]/300);

        chanse = Mathf.Pow(chanse, difficalty);
        float roll = Random.value;
        if (roll<chanse)
        {
            skills[skillname]++;
            print("Skill " + skillname + "'v improved to "+skills[skillname]);
        }
            
    }
    /*public void DeathProtocol()
    {
        print("DEATH protocol is NOT working!!!");
    }*/

}
