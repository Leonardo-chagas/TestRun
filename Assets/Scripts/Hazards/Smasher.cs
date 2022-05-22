using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behaviours for the normal smasher object
public class Smasher : MonoBehaviour
{

    public float speedDown, speedUp, timerFall, timerUp, waitTime;
    public bool wait = false, auto = false;
    public AudioClip smashSound;
    public ObjectShakeHandler shakeHandler;

    public bool fall = false, up = false;
    Vector3 start;
    GameObject hitbox;
    AudioSource audioSource;
    int cont = 0;
    
    void Start()
    {
        start = transform.position;
        hitbox = transform.GetChild(1).gameObject;
        hitbox.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if(!auto){
            if(!wait)
                StartCoroutine("Fall");
            else
                StartCoroutine("Wait");
        }
    }

    
    void Update()
    {
        if(fall){
            transform.Translate(0, -speedDown * Time.deltaTime, 0);
        }
        if(up){
            transform.Translate(0, +speedUp * Time.deltaTime, 0);
        }
        if(transform.position.y >= start.y && cont == 0 && !wait){
            up = false;
            cont += 1;
            if(!auto)
                StartCoroutine("Fall");
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(waitTime);
        wait = false;
        StartCoroutine("Fall");
    }

    IEnumerator Fall(){
        yield return new WaitForSeconds(timerFall);
        StartFall();
    }

    public void StartFall(){
        audioSource.Play();
        fall = true;
        hitbox.SetActive(true);
    }

    IEnumerator Up(){
        yield return new WaitForSeconds(timerUp);
        up = true;
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.layer == 6){
            fall = false;
            audioSource.Stop();
            audioSource.PlayOneShot(smashSound, audioSource.volume);
            shakeHandler.ObjectShake();
            cont = 0;
            hitbox.SetActive(false);
            if(!auto)
                StartCoroutine("Up");
        }
    }
}
