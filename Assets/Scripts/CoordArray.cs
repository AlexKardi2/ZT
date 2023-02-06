using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordArray : MonoBehaviour
{
    // Start is called before the first frame update
    static int xSize = Location.xSize;
    static int ySize = Location.ySize;
    public static float[,,] cArray = new float [xSize,ySize,2];


    void Awake()
    {
        //Задаем все координаты Y у РЯДОВ
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 0, 1] = -4.56f; //ряд 0
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 1, 1] = -3.921f; //ряд 1
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 2, 1] = -3.28f; //ряд 2
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 3, 1] = -2.639f; //ряд 3
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 4, 1] = -1.996f; //ряд 4
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 5, 1] = -1.3533f; //ряд 5
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 6, 1] = -0.713f; //ряд 6
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 7, 1] = -0.073f; //ряд 7
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 8, 1] = 0.5737f; //ряд 8
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 9, 1] = 1.2162f; //ряд 9
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 10, 1] = 1.857f; //ряд 10 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 11, 1] = 2.498f; //ряд 11 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 12, 1] = 3.142f; //ряд 12 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 13, 1] = 3.7869f; //ряд 13 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 14, 1] = 4.4253f; //ряд 14 
        }

        //Задаем все координаты X у СТОЛБцов
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 0
            if (i % 2 == 0) cArray[0, i, 0] = -3.758f; //в четных рядах
            if (i % 2 == 1) cArray[0, i, 0] = -3.386f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 1
            if (i % 2 == 0) cArray[1, i, 0] = -3.012f; //в четных рядах
            if (i % 2 == 1) cArray[1, i, 0] = -2.631f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 2
            if (i % 2 == 0) cArray[2, i, 0] = -2.259f; //в четных рядах
            if (i % 2 == 1) cArray[2, i, 0] = -1.8801f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 3
            if (i % 2 == 0) cArray[3, i, 0] = -1.509f; //в четных рядах
            if (i % 2 == 1) cArray[3, i, 0] = -1.1296f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 4
            if (i % 2 == 0) cArray[4, i, 0] = -0.7489f; //в четных рядах
            if (i % 2 == 1) cArray[4, i, 0] = -0.3749f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 5
            if (i % 2 == 0) cArray[5, i, 0] = 0f; //в четных рядах
            if (i % 2 == 1) cArray[5, i, 0] = 0.378f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 6
            if (i % 2 == 0) cArray[6, i, 0] = 0.749f; //в четных рядах
            if (i % 2 == 1) cArray[6, i, 0] = 1.131f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 7
            if (i % 2 == 0) cArray[7, i, 0] = 1.502f; //в четных рядах
            if (i % 2 == 1) cArray[7, i, 0] = 1.879f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 8
            if (i % 2 == 0) cArray[8, i, 0] = 2.255f; //в четных рядах
            if (i % 2 == 1) cArray[8, i, 0] = 2.6319f;  //нечетных рядах
        }
        for (int i = 0; i < ySize; i++)
        {                                          //столбец 9
            if (i % 2 == 0) cArray[9, i, 0] = 3.004f; //в четных рядах
            if (i % 2 == 1) cArray[9, i, 0] = 3.387f;  //нечетных рядах
        }


        //print("Координаты 4,19 следующие: x = "+cArray[4,19,0]+", у = "+cArray[4,19,1]);
    }

    public static Vector3 HexCenter(int x, int y)
    {
        if ((x < 0) || y < 0 || x > xSize || y > ySize)
            return Vector3.zero;
        else
            return new Vector3(cArray[x, y, 0], cArray[x, y, 1]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
