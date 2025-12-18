using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSoundScript : MonoBehaviour
{
    //Implementation for sound
    public AudioSource audioSource;
    public AudioClip smallWin;
    public AudioClip bigWin;
    public AudioClip sadFish;
    public AudioClip up;
    public AudioClip down;
    public AudioClip left;
    public AudioClip right;
    public AudioClip goodJob;
    public AudioClip wrong;

    //Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip soundName)
    {
        audioSource.PlayOneShot(soundName);
    }

    //Called after winning the minigame
    public void CallFishSound(bool caughtGreatFish, bool caughtFish)
    {
        if (caughtGreatFish)
        {
            PlaySound(bigWin);
        }
        else if(caughtFish)
        {
            PlaySound(smallWin);
        }
        else
        {
            PlaySound(sadFish);
        }
    }

    //Called after a correct up press
    public void CallUpSound()
    {
        PlaySound(up);
    }

    //Called after a correct down press
    public void CallDownSound()
    {
        PlaySound(down);
    }
    //Called after a correct left press
    public void CallLeftSound()
    {
        PlaySound(left);
    }
    //Called after a correct right press
    public void CallRightSound()
    {
        PlaySound(right);
    }
    //Called successfully completing a pattern
    public void CallGoodJobSound()
    {
        PlaySound(goodJob);
    }
    //Called after pressing the wrong key
    public void CallWrongSound()
    {
        PlaySound(wrong);
    }
}
