using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Animation of object going up and down
public class BobbingAnimation : MonoBehaviour
{
    public float speed = 0.5f;
    public float distance = 0.2f;
    float start;
    void Start()
    {
        start = transform.position.y;
    }

    
    void Update()
    {
        if(transform.position.y < start - distance || transform.position.y > start + distance){
            speed = -speed;
        }
        transform.Translate(Vector2.up*speed*Time.deltaTime);
    }
}
