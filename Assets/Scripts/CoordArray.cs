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
        //������ ��� ���������� Y � �����
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 0, 1] = -4.56f; //��� 0
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 1, 1] = -3.921f; //��� 1
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 2, 1] = -3.28f; //��� 2
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 3, 1] = -2.639f; //��� 3
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 4, 1] = -1.996f; //��� 4
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 5, 1] = -1.3533f; //��� 5
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 6, 1] = -0.713f; //��� 6
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 7, 1] = -0.073f; //��� 7
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 8, 1] = 0.5737f; //��� 8
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 9, 1] = 1.2162f; //��� 9
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 10, 1] = 1.857f; //��� 10 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 11, 1] = 2.498f; //��� 11 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 12, 1] = 3.142f; //��� 12 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 13, 1] = 3.7869f; //��� 13 
        }
        for (int i = 0; i < xSize; i++)
        {
            cArray[i, 14, 1] = 4.4253f; //��� 14 
        }

        //������ ��� ���������� X � ��������
        for (int i = 0; i < ySize; i++)
        {                                          //������� 0
            if (i % 2 == 0) cArray[0, i, 0] = -3.758f; //� ������ �����
            if (i % 2 == 1) cArray[0, i, 0] = -3.386f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 1
            if (i % 2 == 0) cArray[1, i, 0] = -3.012f; //� ������ �����
            if (i % 2 == 1) cArray[1, i, 0] = -2.631f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 2
            if (i % 2 == 0) cArray[2, i, 0] = -2.259f; //� ������ �����
            if (i % 2 == 1) cArray[2, i, 0] = -1.8801f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 3
            if (i % 2 == 0) cArray[3, i, 0] = -1.509f; //� ������ �����
            if (i % 2 == 1) cArray[3, i, 0] = -1.1296f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 4
            if (i % 2 == 0) cArray[4, i, 0] = -0.7489f; //� ������ �����
            if (i % 2 == 1) cArray[4, i, 0] = -0.3749f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 5
            if (i % 2 == 0) cArray[5, i, 0] = 0f; //� ������ �����
            if (i % 2 == 1) cArray[5, i, 0] = 0.378f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 6
            if (i % 2 == 0) cArray[6, i, 0] = 0.749f; //� ������ �����
            if (i % 2 == 1) cArray[6, i, 0] = 1.131f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 7
            if (i % 2 == 0) cArray[7, i, 0] = 1.502f; //� ������ �����
            if (i % 2 == 1) cArray[7, i, 0] = 1.879f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 8
            if (i % 2 == 0) cArray[8, i, 0] = 2.255f; //� ������ �����
            if (i % 2 == 1) cArray[8, i, 0] = 2.6319f;  //�������� �����
        }
        for (int i = 0; i < ySize; i++)
        {                                          //������� 9
            if (i % 2 == 0) cArray[9, i, 0] = 3.004f; //� ������ �����
            if (i % 2 == 1) cArray[9, i, 0] = 3.387f;  //�������� �����
        }


        //print("���������� 4,19 ���������: x = "+cArray[4,19,0]+", � = "+cArray[4,19,1]);
    }

    public static Vector3 HexCenter(int x, int y)
    {
        
        return new Vector3(cArray[x, y, 0], cArray[x, y, 1]);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0)) //TODO Wrap it to method somewhere
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xOne = (HexCenter(1, 0) - HexCenter(0, 0)).x;
            float yOne = (HexCenter(0, 1) - HexCenter(0, 0)).y;
            int firstX = (int)((mousePosition.x - HexCenter(0, 0).x) / xOne);
            int lastX = firstX + 2;
            int firstY = (int)((mousePosition.y - HexCenter(0, 0).y) / yOne);
            int lastY = firstY + 1;
            float minDistance = float.MaxValue;
            int foundX=-1;
            int foundY=-1;

            for (int x = firstX; x <= lastX; x++)
            {
                for (int y=firstY; y<=lastY; y++)
                {
                    float distance = (mousePosition - (Vector2)HexCenter(x, y)).magnitude;
                    if (distance<minDistance)
                    {
                        minDistance = distance;
                        foundX = x;
                        foundY = y;
                    }
                        
                }
            }    
             print(foundX + " " + foundY);
        }*/

    }
}
