using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//functions for the pause menu
public class PauseMenu : MonoBehaviour
{
    public void Resume(){
        Pause pause = GameObject.Find("PauseManager").GetComponent<Pause>();
        pause.PauseGame();
    }

    public void Options(){

    }

    public void Exit(){
        SceneManager.LoadScene("menu");
    }
}
