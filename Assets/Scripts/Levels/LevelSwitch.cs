using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public void LevelChange(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
