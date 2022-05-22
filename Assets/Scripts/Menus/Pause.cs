using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that pauses and unpauses the game
public class Pause : MonoBehaviour
{
    public bool isPaused = true;
    //public GameObject[] menus;
    public static Pause instance;

    void Start(){
        instance = this;
    }

    public void PauseGame(){
        if(isPaused){
            Time.timeScale = 0;
            //menus[0].SetActive(true);
            MenuInput.instance.EnableControls();
            MenuInput.instance.EnablePauseMenu();
            isPaused = !isPaused;
        }
        else{
            Time.timeScale = 1;
            /* foreach(GameObject menu in menus)
                menu.SetActive(false); */
            MenuInput.instance.DisablePauseMenu();
            MenuInput.instance.DisableControls();
            isPaused = !isPaused;
        }
    }
}
