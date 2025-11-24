using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This file is a shared state that will hold flags of what is currenlty being done

public class ActionDetector : MonoBehaviour
{
    public bool isRunning = false;   //: whether the cat is running
    public bool isPP = false;      //: Whether the cat is pushing or pulling
    public bool isHit = false;     //: Whether the cat was hit by Thwump
    public bool gotBadFish = false;  //: Wether the player lost the minigame and got the bad fish

    //minigame related values
    public bool wonMiniGame = false;
    public bool smallRestore = false;    //: got small fish from minigame
    public bool bigRestore = false; //: got big fish from minigame
}
