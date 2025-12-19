using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public static float speed = 0.1f;
    public static float maxHeight = 2.5f;
    public static Vector3 defaultPosition = new Vector3(130f, 0f, 188.27f);

    //Start is called before the first frame update
    void Start()
    {
        transform.position = defaultPosition;
    }

    void FixedUpdate()
    {   
        //Slowly move prefab upwards until it reaches a certain height
        if(transform.position.y <= maxHeight)
        {
            float movement = transform.position.y + speed;
            transform.position = new Vector3(transform.position.x, movement, transform.position.z);
        }
        
    }
}