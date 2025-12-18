using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentPlayerData : MonoBehaviour
{
    public static PersistentPlayerData Instance;

    //the last spot the player was in 
    public Vector3 savedPlayerPosition;
    //public bool hasPosition = false; //for detecting position ex

    // using Awake for initiliazation
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // INITIAL DEFAULT POSITION
        savedPlayerPosition = new Vector3(0f, 0f, 0f);
        //hasPosition = false;  // false until explicitly saved

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
