using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This file is a shared state that will hold flags of what is currenlty being done

public class ActionDetector : MonoBehaviour
{
    public bool isWalking = false;   //: whether the cat is running 
    public bool isRunning = false; 
    public bool isPP = false;      //: Whether the cat is pushing or pulling
    public bool isHit = false;     //: Whether the cat was hit by Thwump

}
