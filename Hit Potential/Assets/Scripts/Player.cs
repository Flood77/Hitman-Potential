using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;

    void Start()
    {
        
    }

    void Update()
    {
        //Rotate player to where the mouse is 
        //move player based on wasd
        if (Input.GetKey(KeyCode.W))
        {
            var temp = transform.position;
            temp.y += speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.S))
        {
            var temp = transform.position;
            temp.y -= speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.A))
        {
            var temp = transform.position;
            temp.x += speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.D))
        {
            var temp = transform.position;
            temp.x -= speed;
            transform.position = temp;
        }
    }
}
