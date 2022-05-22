using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behaviours for the laser aspect of the laser generator
public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform laserPoint;
    GameObject startvfx, endvfx;
    BoxCollider2D box;
    AudioSource audioSource;
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    void Start()
    {
        foreach(Transform child in transform){
            if(child.gameObject.name == "LaserPoint")
                laserPoint = child.gameObject.GetComponent<Transform>();
            else if(child.gameObject.name == "StartVFX")
                startvfx = child.gameObject;
            else if(child.gameObject.name == "EndVFX")
                endvfx = child.gameObject;
        }
        lineRenderer = laserPoint.GetChild(0).GetComponent<LineRenderer>();
        box = laserPoint.GetChild(1).GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        FillLists();
        EnableLaser();
        UpdateLaser();
    }

    void Update(){
        UpdateLaser();
    }

    void EnableLaser(){
        lineRenderer.enabled = true;
        audioSource.Play();

        for(int i=0; i<particles.Count; i++){
            particles[i].Play();
        }
    }

    void DisableLaser(){
        lineRenderer.enabled = false;
        audioSource.Stop();

        for(int i=0; i<particles.Count; i++){
            particles[i].Stop();
        }
    }

    void UpdateLaser(){
        lineRenderer.SetPosition(0, new Vector2(laserPoint.localPosition.x , laserPoint.localPosition.y - laserPoint.localPosition.y));
        startvfx.transform.position = (Vector2)laserPoint.position;

        int mask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Raycast((Vector2)laserPoint.position, -transform.up, 50f, mask);

        if(hit.collider){
            Vector3 point = transform.InverseTransformPoint(hit.point);
            lineRenderer.SetPosition(1, new Vector2(point.x, point.y - laserPoint.localPosition.y));
            endvfx.transform.position = hit.point;
            float lineLength = Vector2.Distance((Vector2)laserPoint.localPosition, (Vector2)point);
            Vector2 midPoint = ((Vector2)laserPoint.localPosition + (Vector2)point)/2;
            box.size = new Vector2(box.size.x, lineLength);
            box.transform.localPosition = new Vector2(midPoint.x, midPoint.y + 0.14f);
        }
    }

    void FillLists(){
        for(int i=0; i<startvfx.transform.childCount; i++){
            var ps = startvfx.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps!=null){
                particles.Add(ps);
            }
        }
        for(int i=0; i<endvfx.transform.childCount; i++){
            var ps = endvfx.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps!=null){
                particles.Add(ps);
            }
        }
    }
}
