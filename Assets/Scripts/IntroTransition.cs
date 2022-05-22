using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTransition : MonoBehaviour
{
    public void SwitchToMenu(){
        SceneManager.LoadScene(1);
    }
}
