using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviours for the dash recharge item
public class Orb : MonoBehaviour
{
    bool active = true;
    Animator animator;
    public GameObject light;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player") && active){
            active = false;
            light.SetActive(false);
            animator.SetBool("active", false);
            Player1 player = col.gameObject.GetComponent<Player1>();
            player.Refresh();
            StartCoroutine("Cooldown");
        }
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(5.0f);
        active = true;
        light.SetActive(true);
        animator.SetBool("active", true);
    }
}
