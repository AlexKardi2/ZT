using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour
{
    private void Start()
    {

    }

    public static List<int[]> Get6CloseTiles(CombatCharacter character)
    {
        return Get6CloseTiles(character.pos[0], character.pos[1]);
    }

     public static List<int[]> Get6CloseTiles(int x, int y)
    {
        List<int[]> list = new List<int[]>();
        int[] xOddCorrArray = new int[] { 1, 1, 1, 0, -1, 0 }; //for Odd row
        int[] xEvenCorrArray = new int[] { 0, 1, 0, -1, -1, -1 }; //for Even row
        int[] yCorrArray = new int[] { 1, 0, -1, -1, 0, 1 };
        
        for (int i=0; i<6; i++)
        {
            int[] tileCoords = new int[2];
            tileCoords[1] = y+yCorrArray[i];

            if (y%2==0)
            {
                tileCoords[0] = x + xEvenCorrArray[i];
            } else
            {
                tileCoords[0] = x + xOddCorrArray[i];
            }

            //print((tileCoords[0]-x)+" "+(tileCoords[1]-y));

            list.Add(tileCoords);
        }
        
        return list;
    }

    public static int[] TileToTarget (int[] from, int[] to)
    {
        int[] step = new int[2]; //Returnable coordinate array
        int dist = 999999; //Starting ultra large value
        
        List<int[]> closeTiles = Get6CloseTiles(from[0], from[1]);

        for (int i=0; i<6; i++)
        {
            int xCurrentDist = Mathf.Abs(closeTiles[i][0] - to[0]);
            int yCurrentDist= Mathf.Abs(closeTiles[i][1] - to[1]);
            int currentDist = xCurrentDist + yCurrentDist;
            if (currentDist < dist)
            {
                dist = currentDist;
                step[0] = closeTiles[i][0];
                step[1] = closeTiles[i][1];
            }
                
                
        }

        
        return step;
    }

    public static int FindDistance (int[] from, int[] to)
    {
        int distance = 0;

        if ((from[0] == to[0]) && (from[1] == to[1]))
            return 0;

        int[] movingTile = new int[2];
        movingTile[0] = from[0];
        movingTile[1] = from[1];

        while ( !((movingTile[0] == to[0]) && (movingTile[1] == to[1])))
        {
            movingTile = TileToTarget(movingTile, to);
            //print("Current tile " + movingTile[0] + " " + movingTile[1]);
            distance++;
            if (distance == 55) break;
        }

        return distance;
    }

    public static void Ai (CombatCharacter bot, string ai="rat")
    {
        print("Bot for " + bot.name + " started.");
        //ADD choosing enemy
        CombatCharacter enemy = CombatCharacter.cCList[0];

        bool enoughOD = true;
        int i = 0;
        
        while (enoughOD)
        {
            if (Scripts.FindDistance(bot.planningPos,enemy.pos)<=1) {
                enoughOD = CombatAction.Attack(bot, enemy);
                print("'v planned to ATTACK");
            } else
            {
                bool check=false;
                int[] moveCoordinates = Scripts.TileToTarget(bot.planningPos, enemy.pos);
                bot.personalPlanningList.Add(new CombatAction());
                enoughOD= bot.personalPlanningList[(bot.personalPlanningList.Count - 1)].Move(bot, moveCoordinates[0], moveCoordinates[1]);
                print("'v planned to move to " + moveCoordinates[0] + " " + moveCoordinates[1]);
                if (!enoughOD) bot.personalPlanningList.RemoveAt(bot.personalPlanningList.Count - 1);

                //Moving plan position and sprite
                bot.planningPos[0] = moveCoordinates[0];
                bot.planningPos[1] = moveCoordinates[1];
            }
            i++;
            if (i > 10) break;  
        }

        


    }
        
}
