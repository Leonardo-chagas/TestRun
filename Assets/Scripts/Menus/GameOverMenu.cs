using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//resets the scene if the player presses a button
public class GameOverMenu : MonoBehaviour
{   
    void Update()
    {
       if(Input.anyKeyDown){
           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       } 
    }
}
