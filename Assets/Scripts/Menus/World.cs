using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes the animation of moving the worlds left to right based on an index of positions
public class World : MonoBehaviour
{
    bool right = false;
    bool left = false;
    LevelLock levelLock;
    public bool isUnlocked;
    public Transform[] points;
    public int index;
    public float speed;
    void Start()
    {
        transform.position = points[index].position;
        levelLock = GetComponent<LevelLock>();
        isUnlocked = levelLock.unlocked;
    }

    
    void Update()
    {
        if(right){
            if(Vector2.Distance(transform.position, points[index].position) > .1f)
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            else{
                right = false;
            }
        }
        if(left){
            if(Vector2.Distance(transform.position, points[index].position) > .1f)
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            else{
                left = false;
            }
        }
    }

    public void MoveRight(){
        if(index==4) return;
        index+=1;
        left = false;
        right = true;
    }

    public void MoveLeft(){
        if(index==0) return;
        index-=1;
        left = true;
        right = false;
    }
}
