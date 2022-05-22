using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls every aspect of the music including:
//volume, changing song, stopping song, fade in, etc.
public class MusicPlayer : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    public static MusicPlayer instance;
   
    void Awake()
    {
        if(instance == null){
            DontDestroyOnLoad(transform.gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    
    public void PlayMusic(){
        audioSource.Play();
    }

    public void StopMusic(){
        audioSource.Stop();
    }

    public void ChangeMusic(AudioClip music){
        audioSource.clip = music;
    }

    public IEnumerator FadeIn(float delay){
        yield return new WaitForSeconds(delay);
        audioSource.volume += 0.01f;
        if(audioSource.volume < 1)
            StartCoroutine(FadeIn(delay));
    }

    public IEnumerator FadeOut(float delay){
        yield return new WaitForSeconds(delay);
        audioSource.volume -= 0.01f;
        if(audioSource.volume > 0)
            StartCoroutine(FadeOut(delay));
    }

    public void FullVolume(){
        audioSource.volume = 1;
    }

    public void ChangeVolume(float volume){
        audioSource.volume = volume;
    }

    public void NoVolume(){
        audioSource.volume = 0;
    }
}
