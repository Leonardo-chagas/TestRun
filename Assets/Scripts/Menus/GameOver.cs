using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shows the game over screen if the player has died
public class GameOver : MonoBehaviour
{
    GameObject gameOverMenu;
    void Start()
    {
        Transform ui = GameObject.Find("UI").GetComponent<Transform>();
        foreach(Transform child in ui){
            if(child.gameObject.name == "Game Over Menu"){
                gameOverMenu = child.gameObject;
                gameOverMenu.SetActive(false);
            }
        }
    }

    
    public void GameOverScreen(){
        gameOverMenu.SetActive(true);
    }
}
