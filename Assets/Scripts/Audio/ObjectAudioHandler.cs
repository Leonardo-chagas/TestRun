using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Changes the volume of a sound depending on the distance to the player
public class ObjectAudioHandler : MonoBehaviour
{
    Transform subject;
    public float max;
    public float decay;
    public AudioSource audioSource;
    
    void Start(){
        subject = Player1.instance.transform;
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }
    
    void Update()
    {
        float distance = Mathf.Abs(Vector2.Distance(transform.position, subject.position));
        float maxVolume = audioSource.volume;

        if(distance < max){
            audioSource.volume = maxVolume;
        }
        else{
            audioSource.volume = maxVolume-(distance-max)*decay*maxVolume;
        }
    }
}
