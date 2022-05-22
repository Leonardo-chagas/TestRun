using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shows sprite and animation of button on the screen as a tutorial
public class Tutorial : MonoBehaviour
{
    public string tutorial;
    bool inside = false;
    Transform point;
    GameObject input;
    GameObject button;
    CurrentControlScheme control;
    void Start()
    {
        point = transform.GetChild(0);
        control = GameObject.Find("InputManager").GetComponent<CurrentControlScheme>();
        ChangeInput();
    }

    public void ChangeInput(){
        if(tutorial == "jump")
            input = control.controller.jumpButton;
        else if(tutorial == "walk")
            input = control.controller.walkRightButton;
        else if(tutorial == "dash")
            input = control.controller.dashButton;
        
        if(inside){
            foreach(Transform child in transform){
                if(child.gameObject.CompareTag("button")){
                    Destroy(child.gameObject);
                    GameObject button = Instantiate(input, point.position, point.rotation, transform);
                    Animator buttonAnimator = button.GetComponent<Animator>();
                    buttonAnimator.SetBool("pressed", true);
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            GameObject button = Instantiate(input, point.position, point.rotation, transform);
            Animator buttonAnimator = button.GetComponent<Animator>();
            buttonAnimator.SetBool("pressed", true);
            inside = true;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            foreach(Transform child in transform){
                if(child.gameObject.CompareTag("button")){
                    Destroy(child.gameObject);
                }
            }
            inside = false;
        }
    }
}
