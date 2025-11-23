using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tester script just to help with testing stamina related function
public class MovementSkeleton : MonoBehaviour
{
    //references
    public ActionDetector action;
    // Update is called once per frame
    void Update()
    {
        action.isHit = Input.GetKeyDown("h");
        action.isPP = Input.GetKey("e");
        action.isRunning = Input.GetKey("w") || Input.GetKey("a") ||
                            Input.GetKey("s") || Input.GetKey("d");

        action.gotBadFish = Input.GetKeyDown("b");
        action.smallRestore = Input.GetKeyDown("f");
        action.bigRestore = Input.GetKeyDown("g");

        if (action.isHit)
        {
            Debug.Log("Got hit");
        }

        if (action.isPP)
        {
            Debug.Log("pushing/pulling");
        }

        if (action.isRunning)
        {
            Debug.Log("running somewhere");
        }

        if (action.gotBadFish)
        {
            Debug.Log("got bad fish");
        }

        if (action.smallRestore)
        {
            action.wonMiniGame = true;
            Debug.Log("small restore");
        }

        if (action.bigRestore)
        {
            action.wonMiniGame = true;
            Debug.Log("big restore");
        }
    }
}
