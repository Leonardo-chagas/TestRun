using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    Light2D light;
    public float intensityMax, intensityMin, dimValue;
    int multiplier = 1;
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    
    void Update()
    {
        if(light.intensity >= intensityMax && multiplier > 0 || light.intensity <= intensityMin && multiplier < 0){
            multiplier = -multiplier;
        }
        light.intensity += dimValue*multiplier*Time.deltaTime;
    }
}
