using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//behaviours of object that launches the player on the direction shown by the 2 arrows
public class Launcher : MonoBehaviour
{
    public float force, ringSpeed, cooldown;
    public float dirX, dirY;
    public AudioClip launchSound;
    public GameObject light;

    ParticleSystem ring;
    ParticleSystem.ShapeModule ringRadius;
    bool activate = false, active = true;
    Animator animator;
    AudioSource audioSource;

    void Start(){
        ring = transform.GetChild(1).GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        ringRadius = ring.shape;
        animator = GetComponent<Animator>();
    }

    void Update(){
        if(activate){
            ringRadius.radius = ringRadius.radius + ringSpeed * Time.deltaTime;
        }
    }
    
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player") && active){
            int posY = col.transform.position.y >= transform.position.y ? 1 : -1;
            active = false;
            ring.Play();
            activate = true;
            animator.SetBool("active", false);
            Player1 player = col.transform.GetComponent<Player1>();
            if(player.isDashing)
                player.CancelDash();
            player.velocity.y = 0;
            float x = (player.velocity.x != 0 && dirX == 0) ? player.velocity.x : 0;
            float y = 1;
            /* if(player.velocity.y != 0)
                y = posY > 0 ? 0.9f : 1.10f; */
            player.transform.position = transform.position;
            player.velocity = new Vector3(force*dirX+x, force*dirY*y, 0);
            audioSource.PlayOneShot(launchSound, audioSource.volume);
            StartCoroutine("Ring");
            StartCoroutine("Cooldown");
        }
    }

    IEnumerator Ring(){
        yield return new WaitForSeconds(0.5f);
        activate = false;
        ringRadius.radius = 0.5f;
        ring.Stop();
    }

    IEnumerator Cooldown(){
        light.SetActive(false);
        yield return new WaitForSeconds(cooldown-0.3f);
        animator.SetBool("active", true);
        yield return new WaitForSeconds(0.3f);
        light.SetActive(true);
        active = true;
    }
}
