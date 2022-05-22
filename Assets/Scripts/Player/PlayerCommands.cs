using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//assigns functions to player input
public class PlayerCommands : MonoBehaviour
{
    PlayerControls controls;
    Player player;
    //Pause pause;
    bool jumping = false;

    void Awake(){
        controls = BindingManager.inputActions;
        player = GetComponent<Player>();
        //GameObject obj = GameObject.Find("PauseManager");
        //pause = obj.GetComponent<Pause>();
    }

    void OnEnable(){
        controls.Enable();
    }

    void OnDisable(){
        controls.Disable();
    }

    void Start()
    {
        controls.Player.Dash.performed += _ => player.Dash();
        controls.Player.Jump.performed += _ => player.Jump();
        controls.Player.Jump.canceled += _ => player.JumpCancel();
        controls.Player.Pause.performed += _ => Pause.instance.PauseGame();
        controls.Player.Up.performed += _ => player.directionY = 1;
        controls.Player.Up.canceled += _ => player.directionY = 0;
    }

    
    void Update()
    {
        if(Pause.instance.isPaused)
            player.direction = controls.Player.Move.ReadValue<float>();
        player.isPaused = Pause.instance.isPaused;
    }
}
