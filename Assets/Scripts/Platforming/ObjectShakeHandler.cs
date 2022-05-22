using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//changes the amount of screen shake an object causes depending on its distance to the player
public class ObjectShakeHandler : MonoBehaviour
{
    Transform subject;
    public float max;
    public float decay;
    public ScreenShake screenShakeControler;

    void Start(){
        subject = Player1.instance.transform;
    }

    public void ObjectShake(){
        float distance = Mathf.Abs(Vector2.Distance(transform.position, subject.position));

        if(distance < max){
            screenShakeControler.Shake(1.0f);
        }
        else{
            screenShakeControler.Shake(1-(distance-max)*decay);
        }
    }
}
