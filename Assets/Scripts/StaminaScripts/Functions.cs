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
    public PersistentPlayerData playerData;

    public float currStamina;
    public float delay = 0.45f;

    private void Start()
    {
        //initialize the current stamina to the maximum amount
        currStamina = playerData.savedStamina;
    }

    // Update is called once per frame
    void Update()
    {
        float cost = 0f; // the current cost to stamina
        float restore = 0f; //current amount to restore

        //gets the correct drain amount
        cost = manager.getDrainAmt(action.isWalking, action.isRunning, action.isPP, action.isHit, action.gotBadFish);

        //get the correct restore amount
        restore = manager.getRestoreAmt(action.smallRestore, action.bigRestore);

        //apply it smoothly for smooth actions
        if (action.isWalking || action.isPP || action.isRunning)
        {
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
            Debug.Log($"small victory{action.smallRestore}");
            Debug.Log($"big victory{action.bigRestore}");
            Debug.Log($"Pre restore stamina {currStamina}");
            currStamina += restore;
            Debug.Log($"post restore stamina {currStamina}");


            //reset flags
            action.wonMiniGame = false;
            action.smallRestore = false;
            action.bigRestore = false;
            Debug.Log("reset check:");
            Debug.Log($"small victory{action.smallRestore}");
            Debug.Log($"big victory{action.bigRestore}");
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