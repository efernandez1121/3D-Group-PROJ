using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideColliders : MonoBehaviour
{
    public thwompMovement mainThwomp;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainThwomp.collisionActions();
        }
    }
}
