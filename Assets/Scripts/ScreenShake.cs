using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//gets the screen shake power from the player settings and creates screen shake when function is called
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private CinemachineImpulseDefinition impulseDefinition;
    float screenShake => PlayerPrefs.GetFloat("ScreenShakePower", 1.0f);
    
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        //impulseDefinition = GetComponent<CinemachineImpulseDefinition>();
        impulseDefinition = impulseSource.m_ImpulseDefinition;
    }

    
    public void Shake(float multiplier){
        impulseDefinition.m_AmplitudeGain = screenShake*multiplier;
        impulseSource.GenerateImpulse();
    }
}
