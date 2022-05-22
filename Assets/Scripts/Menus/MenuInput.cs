using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//functions for the controller input while on the menus
public class MenuInput : MonoBehaviour
{
    MenuController menuController;
    public static MenuInput instance;
    public EventSystem eventSystem;
    public GameObject firstButton;
    public GameObject[] menus;
    public Button[] backCommands;
    public Button[] rightCommands;
    public Button[] leftCommands;
    void Awake()
    {
        menuController = new MenuController();
        instance = this;
    }

    public void EnableControls(){
        menuController.Enable();
    }

    public void DisableControls(){
        menuController.Disable();
    }

    void Start(){
        menuController.UI.Back.performed += _ => Back();
        menuController.UI.Right.performed += _ => Right();
        menuController.UI.Left.performed += _ => Left();
    }

    void Back(){
        foreach(Button button in backCommands){
            if(button.gameObject.activeInHierarchy){
                button.onClick.Invoke();
                break;
            }
        }
    }

    void Right(){
        foreach(Button button in rightCommands){
            if(button.gameObject.activeInHierarchy)
                button.onClick.Invoke();
                break;
        }
    }

    void Left(){
        foreach(Button button in leftCommands){
            if(button.gameObject.activeInHierarchy)
                button.onClick.Invoke();
                break;
        }
    }

    public void EnablePauseMenu(){
        menus[0].SetActive(true);
        eventSystem.SetSelectedGameObject(firstButton);
    }

    public void DisablePauseMenu(){
        foreach(GameObject menu in menus)
            menu.SetActive(false);
    }
}
