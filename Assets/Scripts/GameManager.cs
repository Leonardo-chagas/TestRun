using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool fade = true;

    void Awake(){
        if(fade)
            FadeOut();
    }

    void FadeOut(){
        Animator fade = Camera.main.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        fade.SetBool("fadeOut", true);
    }
}
