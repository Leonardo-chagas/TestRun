using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviours for when the player collides with a trigger or collider
public class PlayerCollisions : MonoBehaviour
{
    Player1 player;
    public GameObject particles;
    public AudioClip deathSound;
    public AudioClip inverterSound;
    public GameObject inverterFlash;
    AudioSource audioSource;
    GameOver gameOver;
    Rigidbody2D rb;
    Animator heightAnimator;
    int dir;

    void Start(){
        player = GetComponent<Player1>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        gameOver = GameObject.Find("GameOverManager").GetComponent<GameOver>();
        foreach(Transform child in transform){
            if(child.gameObject.name == "h"){
                heightAnimator = child.gameObject.GetComponent<Animator>();
            }
        }
    }
    /* void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("moving platform")){
            this.transform.parent = col.transform;
        }
    } */

    void OnCollisionStay2D(Collision2D col){
        if(col.gameObject.layer == 7 && !player.invincible){
            Instantiate(particles, transform.position, transform.rotation);
            audioSource.PlayOneShot(deathSound, audioSource.volume);
            gameOver.GameOverScreen();
            Destroy(GetComponent<CapsuleCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Player>());
            Destroy(GetComponent<PlayerCommands>());

            foreach(Transform child in transform){
                Destroy(child.gameObject);
            }
        }
    }

    /* void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.CompareTag("moving platform")){
            this.transform.parent = null;
        }
    } */

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.layer == 7 && !player.invincible){
            Instantiate(particles, transform.position, transform.rotation);
            audioSource.PlayOneShot(deathSound, audioSource.volume);
            gameOver.GameOverScreen();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Player1>());
            Destroy(GetComponent<PlayerInput>());

            foreach(Transform child in transform){
                Destroy(child.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Inverter")){
            dir = transform.position.x > col.transform.position.x ? 1 : -1;
            player.StopCoroutine("ChangeGravity");
            player.StartCoroutine(player.ChangeGravity(0.0f, 0.6f));
        }
        if(col.gameObject.CompareTag("moving platform")){
            this.transform.parent = col.transform;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Inverter")){
            if(transform.position.x > col.transform.position.x && dir < 0 ||
            transform.position.x < col.transform.position.x && dir > 0){
                player.gravityMultiplier = -player.gravityMultiplier;
                //rb.gravityScale = -rb.gravityScale;
                heightAnimator.SetInteger("upside", player.gravityMultiplier);
                audioSource.PlayOneShot(inverterSound, audioSource.volume);
                StartCoroutine("InverterFlash");
            }
        }
        if(col.gameObject.CompareTag("moving platform")){
            this.transform.parent = null;
        }
    }

    IEnumerator InverterFlash(){
        inverterFlash.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        inverterFlash.SetActive(false);
    }
}
