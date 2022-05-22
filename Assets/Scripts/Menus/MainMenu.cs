using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functions for the main menu
public class MainMenu : MonoBehaviour
{
    void Start(){
        PlayerPrefs.SetInt("IntroPassed", 1);
    }
    
    public void Quit(){
        Application.Quit();
    }
}
