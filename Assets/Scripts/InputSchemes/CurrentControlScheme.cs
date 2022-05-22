using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Changes the visuals of the buttons depending on the current controller being used
public class CurrentControlScheme : MonoBehaviour
{
    public CurrentController controller;
    public ControlUIScheme controlScheme;
    
    void Awake()
    {
        GameObject jump = gameObject, walkLeft = gameObject, walkRight = gameObject, dash = gameObject;
        foreach(GameObject button in controlScheme.PlaystationButtons){
            if(button.name == "Playstation South Button"){
                jump = button;
                walkLeft = button;
                walkRight = button;
            }
            if(button.name == "Playstation West Button"){
                dash = button;
            }
        }
        controller = new CurrentController(jump, walkRight, walkLeft, dash);
    }

    public void ChangeToPlaystationControls(){
        foreach(GameObject button in controlScheme.PlaystationButtons){
            if(button.name == "Playstation South Button"){
                controller.jumpButton = button;
                controller.walkLeftButton = button;
                controller.walkRightButton = button;
            }
            if(button.name == "Playstation West Button"){
                controller.dashButton = button;
            }
        }
        foreach(Transform child in transform){
            if(child.gameObject.CompareTag("tutorial")){
                Tutorial tutorial = child.gameObject.GetComponent<Tutorial>();
                tutorial.ChangeInput();
            }
        }
    }

    public void ChangeToXboxControls(){
        foreach(GameObject button in controlScheme.XboxButtons){
            if(button.name == "Xbox South Button"){
                controller.jumpButton = button;
                controller.walkLeftButton = button;
                controller.walkRightButton = button;
            }
            if(button.name == "Xbox West Button"){
                controller.dashButton = button;
            }
        }
        foreach(Transform child in transform){
            if(child.gameObject.CompareTag("tutorial")){
                Tutorial tutorial = child.gameObject.GetComponent<Tutorial>();
                tutorial.ChangeInput();
            }
        }
    }

    public struct CurrentController{
        public GameObject jumpButton;
        public GameObject walkRightButton;
        public GameObject walkLeftButton;
        public GameObject dashButton;

        public CurrentController(GameObject jumpButton, GameObject walkRightButton,
        GameObject walkLeftButton, GameObject dashButton){
            this.jumpButton = jumpButton;
            this.walkRightButton = walkRightButton;
            this.walkLeftButton = walkLeftButton;
            this.dashButton = dashButton;
        }
    }
}
