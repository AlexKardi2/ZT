using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsList : MonoBehaviour
{
    //Singltone var
    public static PrefabsList instance { get; internal set; }
    
    //Prefabs for different NPC
    public GameObject ratPrefab;

    //TEMP prefabs
    public GameObject coordinatesText;

    private void Awake()
    {
        if ((instance != this) && (instance != null))
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


}
