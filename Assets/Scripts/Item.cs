using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    static private float rangeBalansingParametr = 1; //Multiplies max ranges of weapons & characters. Basicly 2 for FalloutPNP. 

    public static List<Item> items = new List<Item>();

    public string itemName;
    public bool rangedAttack = true;
    private int damageRandomTo = 6; //1dX (10 for 1d10)
    private int damageRandomMultipler = 1; //3 for 3d6
    private int damageAddition = 0; //5 for 2d8+5

    public int _range = 2;
    public int Range
    {
        get
        {
            return (int)(_range * (1 / rangeBalansingParametr));
        }
        set {
            _range = value;
        }
        
    }
    public int apCost = 5;
    public string DamageDiapason
    {
        get
        {
            return (damageRandomMultipler + damageAddition) + "-" + (damageRandomMultipler * damageRandomTo + damageAddition);
        }
    }


    public string skillname = "";
    
    // Start is called before the first frame update
    void Awake()
    {
        Item thisItem=new Item();
        thisItem.itemName = "Fist";
        thisItem.SetDamage(1,4,0);
        thisItem.Range = 1;
        thisItem.apCost = 3;
        thisItem.rangedAttack = false;
        thisItem.skillname = "unarmed";
        items.Add(thisItem);

        thisItem = new Item();
        thisItem.itemName = "Rifle";
        thisItem.SetDamage(1, 8, 8);
        thisItem.Range = 19;
        thisItem.apCost = 5;
        thisItem.skillname = "guns";
        items.Add(thisItem);

        thisItem = new Item();
        thisItem.itemName = "Knife";
        thisItem.SetDamage(1, 10, 0);
        thisItem.Range = 1;
        thisItem.apCost = 3;
        thisItem.rangedAttack = false;
        thisItem.skillname = "melee";
        items.Add(thisItem);

        //print(items.Count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Damage
    {
        get
        {
            int summ=0;
            for (int i=0; i<damageRandomMultipler; i++)
            {
                summ += Random.Range(1, (damageRandomTo + 1));
            }
            summ += damageAddition;
            return summ;
        }
    }

    public void SetDamage(int randomMultipler=1, int randomTo=6, int addition=0)
    {
        damageRandomMultipler = randomMultipler;
        damageRandomTo = randomTo;
        damageAddition = addition;
    }

    public void BoostDamage (string parametr="addition", int value=1)
    {
        if (parametr=="addition")
        {
            damageAddition += value;
        } else if (parametr=="dice")
        {
            damageRandomTo += value;
        } else if (parametr=="multipler")
        {
            damageRandomMultipler += value;
        }
    }

}
