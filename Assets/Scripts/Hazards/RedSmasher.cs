using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behaviours for the red smasher object
public class RedSmasher : MonoBehaviour
{
    public float speedDown, speedUp, timerUp;
    public AudioClip smashSound;
    public ObjectShakeHandler shakeHandler;

    bool fall = false, up = false;
    Vector3 start;
    GameObject hitbox;
    Transform point1, point2;
    AudioSource audioSource;
    int cont = 0;
    
    void Start()
    {
        start = transform.position;
        hitbox = transform.GetChild(1).gameObject;
        point1 = transform.GetChild(2).transform;
        point2 = transform.GetChild(3).transform;
        audioSource = GetComponent<AudioSource>();
        hitbox.SetActive(false);
    }

    
    void Update()
    {
        if(fall){
            transform.Translate(0, -speedDown * Time.deltaTime, 0);
        }
        if(up){
            transform.Translate(0, +speedUp * Time.deltaTime, 0);
        }
        if(transform.position.y >= start.y && cont == 0){
            up = false;
            cont += 1;
        }
        if(transform.position.y >= start.y){
            Detect();
        }
    }

    void Detect(){
        int mask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit1 = Physics2D.Raycast(point1.position, -Vector2.up, 50, mask);
        RaycastHit2D hit2 = Physics2D.Raycast(point2.position, -Vector2.up, 50, mask);
        Debug.DrawRay(point1.position, -Vector2.up*50, Color.green);
        Debug.DrawRay(point2.position, -Vector2.up*50, Color.green);

        if((hit1.collider || hit2.collider) && !fall){
            fall = true;
            hitbox.SetActive(true);
            audioSource.Play();
        }
    }

    IEnumerator Up(){
        yield return new WaitForSeconds(timerUp);
        up = true;
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.layer == 6){
            audioSource.Stop();
            audioSource.PlayOneShot(smashSound, audioSource.volume);
            shakeHandler.ObjectShake();
            fall = false;
            cont = 0;
            hitbox.SetActive(false);
            StartCoroutine("Up");
        }
    }
}
