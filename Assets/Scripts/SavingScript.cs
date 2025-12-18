using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingScript : MonoBehaviour
{
    public static SavingScript player;

    // Update is called once per frame
    void Awake()
    {
        if(player != null)
        {
            Destroy(gameObject);
        }
        else
        {
            player = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
