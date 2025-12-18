using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentPlayerData : MonoBehaviour
{
    public ManagerStamina manager;

    public static PersistentPlayerData Instance;

    //the last spot the player was in 
    public Vector3 savedPlayerPosition;
    public float savedStamina;

    //minigame related values
    public bool wonMiniGame = false;
    public bool smallRestore = false;    // got small fish from minigame
    public bool bigRestore = false; // got big fish from minigame
    public bool gotBadFish = false;  // Wether the player lost the minigame and got the bad fish

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
        savedPlayerPosition = new Vector3(0f, 0.5f, 0f);

        //INITIAL DEFAULT STAMINA
        savedStamina = manager.maxStamina;
    }

}
