using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using static UnityEditor.PlayerSettings;

//This file is the functions for movements for the Thwomp - must be attached to the thwomp
public class thwompMovement : MonoBehaviour
{
    //references
    public ActionDetector detector;

    //sound
    private AudioSource audioSource;
    public AudioClip hitAudio;
    public float volume;

    [Header("settings")]
    [Header("Vertical positioning")]
    public float topHeight = 7f;
    public float bottomHeight = 0f;

    [Header("Horizontal-shaking range")]
    public float leftAndRightEdge = 0.35f;

    [Header("Speed and acceleration amountage")]
    public float speed = 10f;
    public float maxSpeed = 20f;
    public float acceleration = 10f;
    public float shakeSpeed = 9f;

    [Header("Delays")]
    //public float bottomDelay = 1.5f;
    public float delay = 3f;
    public float shakeTime = 2f;
    public float soundDelay = 1f;

    [Header("Flags")]
    protected bool movingUp = false;
    protected bool wait = false;
    protected bool shaking = false;

    [Header("Trackers")]
    public float currSpeed = 10f; //same as speed, but it'll be changed later
    public float xStart;
    public float lastHitSound = -999f; //set it so that it def won't trigger immediately

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // makees the thwump the audio source
        xStart = transform.position.x; //get the marker of current x position
    }

    // Update is called once per frame
    void Update()
    {
        if (wait || shaking) return; // don't move while waiting or shaking
        Vector3 pos = transform.position;

        if (movingUp && !wait)
        {
            pos.y += speed * Time.deltaTime; // Should move up smoothly

            //pause at top
            if (pos.y >= topHeight) {
                currSpeed = speed; //resets curr speed for the next time it goes down
                movingUp = false;
                StartCoroutine(topDelay());
            }
        }
        else // going down case
        {
            // should accelerate downwards
            currSpeed += acceleration * Time.deltaTime;

            pos.y -= currSpeed * Time.deltaTime; // should accelerate downwards

            //pause at bottom
            if (pos.y <= bottomHeight)
            {
                movingUp = true;
                StartCoroutine(bottomDelay());
            }
        }
        transform.position = pos;
    }
    private IEnumerator Shake() //fix logic to be like the up down logic
    {
        Debug.Log("Shaking now rahh");
        shaking = true;
        float timeElapsed = 0f; //for keeping track of how long its been shaking
        Vector3 pos = transform.position;
        pos.x = xStart;
        transform.position = pos;
        float direction = 1f;

        float relativeLeftEdge = xStart - leftAndRightEdge;
        float relativeRightEdge = xStart + leftAndRightEdge;


        while (timeElapsed < shakeTime)
        {
            pos = transform.position;
            if (pos.x <= relativeLeftEdge)
            {
                direction = 1f; 
            }
            else if (pos.x >= relativeRightEdge)
            {
                direction = -1f; 
            }

            //apply the shaking
            pos.x += direction * shakeSpeed * Time.deltaTime;
            transform.position = pos;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        shaking = false;
    }
    private IEnumerator topDelay()
    {
        wait= true;
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Shake());
        wait = false;
    }

    private IEnumerator bottomDelay()
    {
        wait = true;
        yield return new WaitForSeconds(delay);
        wait = false;
    }
    // also records decrease instamina
    public void PlayHitSound()
    {
        Debug.Log("playing sound");
        detector.isHit = true;
        //block excessive playing of sound
        if (Time.time - lastHitSound < soundDelay)
            return;   

        lastHitSound = Time.time;
        audioSource.PlayOneShot(hitAudio, volume);
        detector.isHit=false; //reset flag
    }

}
