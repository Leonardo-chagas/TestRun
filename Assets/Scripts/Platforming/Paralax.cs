using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the paralax effect for the backgrounds
public class Paralax : MonoBehaviour
{
    public float offset;

    Camera cam;

    Transform subject;

    Vector2 startPosition;
    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    float distance => transform.position.z - subject.position.z;

    float clippingPlane => (cam.transform.position.z + (distance > 0 ? cam.farClipPlane : cam.nearClipPlane));
    
    float parallaxFactor => Mathf.Abs(distance) / clippingPlane;

   public void Awake(){
       cam = Camera.main;
       subject = GameObject.Find("Player (1)").transform;
       startPosition = transform.position;
       startZ = transform.position.z;
   }

   public void Update(){
       Vector2 newPos = startPosition + travel * parallaxFactor;
       transform.position = new Vector3(newPos.x + offset, newPos.y, startZ);
   }
}
