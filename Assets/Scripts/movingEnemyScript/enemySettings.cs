using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is all the Thwomp enemy's settings and configurable metrics
[CreateAssetMenu(menuName = "Managers/enemySettings")]
public class enemySettings : ScriptableObject
{
    [Header("Vertical positioning")]
    public float topHeight = 7f;
    public float bottomHeight = 0f;

    [Header("Horizontal-shaking range")]
    public float leftAndRightEdge = 0.25f;

    [Header("Speed and acceleration amountage")] 
    public float speed = 10f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float shakeSpeed = 25f;

    [Header("Delays")]
    //public float bottomDelay = 1.5f;
    public float delay = 3f;
    public float shakeTime = 2f;

}
