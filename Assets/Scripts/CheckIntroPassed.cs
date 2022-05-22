using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIntroPassed : MonoBehaviour
{
    
    void Awake()
    {
        if(PlayerPrefs.GetInt("IntroPassed", 1) == 1){
            gameObject.SetActive(false);
        }
    }
}
