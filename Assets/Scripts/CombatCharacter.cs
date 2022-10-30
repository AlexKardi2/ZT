using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour
{
    //List of all Combat Characters in the scenes
    public static List<CombatCharacter> cCList = new List<CombatCharacter>();

    //Variables
    public string charName;
    public bool dead = false;
    public int level = 1;
    public int[] pos=new int[2];
    public bool usesOffHand = false;
    //ADD vars or array or dictionary of Item for equipment

    //Basic stats
    public int ST = 6; //CHANGE to 5 later //Strenght
    public int PE = 3; //CHANGE to 5 later //Perception
    public int EN = 6; //CHANGE to 5 later //Endurance
    public int AG = 8; //CHANGE to 5 later //Agility

    //Secondary stats
    public int totalOD; //Action points (ochki deystvija)
    private int OD;
    public int maxHP;
    public int HP; //Healts points
    public int AC; //Acmor class
    public int MD; //Melee damage

    //AI Stats
    public string ai = "";
    
    //Planning stuff
    public int planningOD=8;
    public int[] planningPos = new int[2];
    public List<CombatAction> personalPlanningList = new List<CombatAction>();

    //setting X & Y corrections for 1st circle + List of child GameObjects
    private int[] xOddCorrArray = new int[] { 1, 1, 1, 0, -1, 0}; //for Odd row
    private int[] xEvenCorrArray = new int[] { 0, 1, 0, -1, -1, -1 }; //for Even row
    private int[] yCorrArray = new int[] { 1, 0, -1, -1, 0, 1};
    public GameObject prefabClickZone;
    private List<GameObject> clickZones=new List<GameObject>();
    public GameObject attackZone;

    //TEMP variables before have created item system
        
    
    // Start is called before the first frame update
    void Awake()
    {
        cCList.Add(this);
    }

    private void Start()
    {
        
        if (ai == "")
            CalculateSecStats();
        ResetOD();
        HP = maxHP;

        //TEMP setting places for Combatcharacters
        int i = cCList.IndexOf(this);
        CombatCharacter.cCList[i].pos[0] = i;
        CombatCharacter.cCList[i].pos[1] = i;

        //This is not temporary part
        ResetPlanning();
        CombatCharacter.cCList[i].transform.position = new Vector3(CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 0], CoordArray.cArray[CombatCharacter.cCList[i].pos[0], CombatCharacter.cCList[i].pos[1], 1], 0);
        CombatCharacter.cCList[i].CreateClickZones();

        //TEMP part continuing
        if (i == 1) CombatCharacter.cCList[1].ai = "rat";
        if (i == (cCList.Count-1))
            CombatCharacter.cCList[Status.player].StartPlanning();

    }

    private void CalculateSecStats()
    {
        if (ST == 0) return; //Check for NPC
        totalOD = (AG / 2) + 5;
        maxHP = 15 + (ST + (2 * EN));
        AC = AG; //ADD here adding AC for armor

    }

    public void CreateClickZones ()
    {
        //Create correct x+y corrections arrays
        int[] xCorrArray = new int[yCorrArray.Length];
        if ((pos[1] % 2) == 0)
            xCorrArray = xEvenCorrArray;
        else
            xCorrArray = xOddCorrArray;

        //Create array for corrections coordinates of ClickZones
        List<float[]> clickzoneCoordCorrections = new List<float[]>(); 
        for (int i=0; i< yCorrArray.Length; i++)
        {
            int x=1 + xOddCorrArray[i];
            int y=1 + yCorrArray[i];
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
            clickZones[i].transform.position = new Vector3((this.transform.position.x+clickzoneCoordCorrections[i][0]), (this.transform.position.y + clickzoneCoordCorrections[i][1]), 0);

            clickZones[i].SetActive(false); //Turning them off
        }

        attackZone = Instantiate<GameObject>(prefabClickZone);
        attackZone.transform.parent = this.transform;
        attackZone.transform.position = new Vector3(CoordArray.cArray[this.pos[0], this.pos[1],0], CoordArray.cArray[this.pos[0], this.pos[1], 1],0);
        attackZone.GetComponent<ClickArea>().combatCharacter = this;
        attackZone.GetComponent<ClickArea>().action = "attack";
        attackZone.SetActive(false);

    }

    public void ResetPlanning ()
    {
        planningOD = totalOD;
        planningPos[0] = pos[0];
        planningPos[1] = pos[1];
        this.transform.position = new Vector3(CoordArray.cArray[this.pos[0], this.pos[1], 0], CoordArray.cArray[this.pos[0], this.pos[1], 1], 0);
    }

    public void StartPlanning (bool start=true)
    {
        if (start && this.ai != "")
        {
            print("I want to start AI");
            Scripts.Ai(this);
            print("AI have finished work");
            Status.NextPlayer();
            return;
        }
        
        for (int i=0; i<clickZones.Count; i++)
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

                bool check=true;
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

    public void ResetOD ()
    {
        OD = totalOD;
    }

    public bool SpendOD (int cost, bool isPlanning=false)
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
        } else
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

    public void MovePlan (int x, int y)
    {
        if ((Mathf.Abs(planningPos[0]-x)>1) || (Mathf.Abs(planningPos[1] - y) > 1))
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

    
}
