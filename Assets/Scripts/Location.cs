using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    //General MAP SIZE
    public static int xSize = 10; 
    public static int ySize = 15;

    //Static Array to get Map information
    public static Location[,] map=new Location[xSize, ySize];

    //Fields of each tile
    int moveOD = 2;

    public int od
    {
        get
        {
            return moveOD;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Fulfilling map
        for (int i =0; i<xSize; i++)
        {
            for (int j=0; j < ySize; j++)
            {
                map[i, j] = new Location();
            }

        }

    }

}
