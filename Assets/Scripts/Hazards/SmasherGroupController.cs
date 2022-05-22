using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherGroupController : MonoBehaviour
{
    public Smasher[] smashers;
    public float interval, waitTime;
    public float speedUp = 6, speedDown = 10;
    bool start = true;
    bool synchronizeUp = false, synchronizeFall = false;

    void Start()
    {
        foreach(Smasher smasher in smashers){
            smasher.auto = true;
            smasher.speedUp = speedUp;
            smasher.speedDown = speedDown;
        }
        StartCoroutine("WaitSequence");
    }

    
    void Update()
    {
        if(synchronizeUp){
            foreach(Smasher smasher in smashers){
                smasher.up = true;
            }
            synchronizeUp = false;
            StartCoroutine("WaitSequence");
        }
    }

    IEnumerator WaitSequence(){
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SmashSequence(0));
    }

    IEnumerator SmashSequence(int index){
        smashers[index].StartFall();
        yield return new WaitForSeconds(interval);
        if(index >= smashers.Length - 1){
            yield return new WaitForSeconds(1.0f);
            synchronizeUp = true;
        }
        else{
            StartCoroutine(SmashSequence(index+1));
        }
    }
}
