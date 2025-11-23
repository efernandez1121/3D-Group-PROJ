using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the configurable, constant stamina values + functions to return restore and dmg amts
[CreateAssetMenu(menuName = "Managers/StaminaManager")]
public class ManagerStamina : ScriptableObject
{
    [Header("Core amts")]
    public float maxStamina = 20f;
    public float minStamina = 0f;

    [Header("Action Costs")]
    public float runCost = 1f;
    public float pushPullCost = 2f;

    [Header("Damage Costs")] //Thwump attacks + bad fish
    public float thwumpDmg = 10f;
    public float badFish = 5f;

    [Header("Restore Amts")] //from fish minigame
    public float smallFish = 5f;
    public float bigFish = 15f;

    //Functions
    //Checks and returns the correct drain amount based on the activity being performed
    // note that as implemented the costs stack; ie running + pulling stacks costs
    public float getDrainAmt(bool isRunning, bool isPP, bool isHit, bool gotBadFish)
    {
        float drainAmt = 0f;

        if (isRunning) {
            drainAmt += runCost;
        }

        if (isPP) {
            drainAmt += pushPullCost;
        }

        if (isHit) //already accounts for if it was hit whilst doing one of the other actions
        {
            drainAmt += thwumpDmg;   
        }

        if (gotBadFish) { 

            drainAmt += badFish;
        }
        return drainAmt;
    }

    //Checks and returns the restoration amount based on what outcome was achieved
    public float getRestoreAmt(bool smallRestore, bool bigRestore)
    {
        if (smallRestore)
        {
            return smallFish;
        }
        if (bigRestore)
        {
            return bigFish;
        }
        else return 0f;
    }

}
