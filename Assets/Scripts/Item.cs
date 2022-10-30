using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static List<Item> items = new List<Item>();
    
    public bool rangedAttack = true;
    private int damageRandomTo = 6; //1dX (10 for 1d10)
    private int damageRandomMultipler = 1; //3 for 3d6
    private int damageAddition = 0; //5 for 2d8+5

    public int range = 2;
    public int odCost = 5;
    
    // Start is called before the first frame update
    void Awake()
    {
        Item thisItem=new Item();
        thisItem.SetDamage(1,4,0);
        thisItem.range = 1;
        thisItem.odCost = 3;
        thisItem.rangedAttack = false;
        items.Add(thisItem);

        thisItem = new Item();
        thisItem.SetDamage(1, 8, 8);
        thisItem.range = 19;
        thisItem.odCost = 5;
        items.Add(thisItem);

        thisItem = new Item();
        thisItem.SetDamage(1, 10, 0);
        thisItem.range = 1;
        thisItem.odCost = 3;
        thisItem.rangedAttack = false;
        items.Add(thisItem);

        //print(items.Count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int damage
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

}
