using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//assigns functions to player input
public class PlayerInput : MonoBehaviour
{
    PlayerControls controls;
    Player1 player;
    //Pause pause;
    bool jumping = false;

    void Awake(){
        controls = BindingManager.inputActions;
        player = GetComponent<Player1>();
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
        controls.Player.Dash.performed += _ => player.isDashing = player.canDash ? true : false;
        controls.Player.Jump.performed += _ => player.OnJumpInputDown();
        controls.Player.Jump.canceled += _ => player.OnJumpInputUp();
        controls.Player.Pause.performed += _ => Pause.instance.PauseGame();
        controls.Player.Up.performed += _ => player.directionY = 1;
        controls.Player.Up.canceled += _ => player.directionY = 0;
    }

    
    void Update()
    {
        if(Pause.instance.isPaused)
            player.SetDirectionalInput(controls.Player.Move.ReadValue<float>());
        player.isPaused = Pause.instance.isPaused;
    }
}