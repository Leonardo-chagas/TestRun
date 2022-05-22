using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntro : MonoBehaviour
{
    
    void Start()
    {
        PlayerPrefs.SetInt("IntroPassed", 0);
    }
}
