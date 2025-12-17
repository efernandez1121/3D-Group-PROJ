using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


//this file handles all the math calculations that'll be running
public class Functions : MonoBehaviour
{
    //reference to initial stats
    public ManagerStamina manager;
    public ActionDetector action; //add GameObject

    public float currStamina;
    public float delay = 0.5f;

    private void Start()
    {
        //initialize the current stamina to the maximum amount
        currStamina = manager.maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        float cost = 0f; // the current cost to stamina
        float restore = 0f; //current amount to restore

        //gets the correct drain amount
        cost = manager.getDrainAmt(action.isRunning, action.isPP, action.isHit, action.gotBadFish);

        //get the correct restore amount
        restore = manager.getRestoreAmt(action.smallRestore, action.bigRestore);
        
        //apply it smoothly for smooth actions
        if (action.isRunning || action.isPP) {
            currStamina -= cost * Time.deltaTime;
        }

        //apply once if its a one time thing
        if (action.isHit || action.gotBadFish)
        {
            currStamina -= cost;
            //reset flags
            action.isHit = false; 
            action.gotBadFish = false;

        }

        //check for stamina restore
        if (action.wonMiniGame)
        {
            currStamina += restore;
            //reset flags
            action.wonMiniGame = false;
        }

        //check for if out of stamina
        if (currStamina <= 0f)
        {
            StartCoroutine(Delay());
        }

        //adjust stamina to make sure that it doesn't go below 0 or above max
        currStamina = UnityEngine.Mathf.Clamp(currStamina, 0, manager.maxStamina);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOver"); 
    }
}
