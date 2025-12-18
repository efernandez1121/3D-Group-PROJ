using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tester script just to help with testing stamina related function
// remember to add asset > audio source to whatever the audio source is for this to work in addition to the code
public class staminaSound : MonoBehaviour
{
    //references
    public ActionDetector action;

    
    //trial implementation for sound
    private AudioSource tempAudioSource;
    public AudioClip hit;
    public AudioClip smallWin;
    public AudioClip bigWin;
    public AudioClip sadFish;

    //sound playing function
    

    private void Start()
    {
        tempAudioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip soundName)
    {
        tempAudioSource.PlayOneShot(soundName);
    }
    // Update is called once per frame
    void Update()
    {
        action.isHit = Input.GetKeyDown("h");
        action.isPP = Input.GetKey("e");
        action.isWalking = Input.GetKey("w") || Input.GetKey("a") ||
                            Input.GetKey("s") || Input.GetKey("d");

        PersistentPlayerData.Instance.gotBadFish = Input.GetKeyDown("b");
        PersistentPlayerData.Instance.smallRestore = Input.GetKeyDown("f");
        PersistentPlayerData.Instance.bigRestore = Input.GetKeyDown("g");

        if (action.isHit)
        {
            Debug.Log("Got hit");
            PlaySound(hit);
        }

        if (action.isPP)
        {
            Debug.Log("pushing/pulling");
        }

        if (action.isWalking)
        {
            Debug.Log("running somewhere");
        }

        if (PersistentPlayerData.Instance.gotBadFish)
        {
            PlaySound(sadFish);
            Debug.Log("got bad fish");
        }

        if (PersistentPlayerData.Instance.smallRestore)
        {
            PersistentPlayerData.Instance.wonMiniGame = true;
            PlaySound(smallWin);
            Debug.Log("small restore");
        }

        if (PersistentPlayerData.Instance.bigRestore)
        {
            PlaySound(bigWin);
            PersistentPlayerData.Instance.wonMiniGame = true;
            Debug.Log("big restore");
        }
    }
}
