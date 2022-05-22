using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//loads the scene belonging to this object if the level is unlocked
public class LevelSelect : MonoBehaviour
{
    public string level;
    LevelLock levelLock;
    void Start(){
        levelLock = GetComponent<LevelLock>();
    }
    public void LevelStart(){
        if(levelLock.unlocked)
            SceneManager.LoadScene(level);
    }
}
