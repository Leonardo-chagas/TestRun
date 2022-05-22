using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Play the music of the level
public class LevelMusic : MonoBehaviour
{
    public AudioClip music1;
    
    void Start()
    {
        MusicPlayer.instance.ChangeMusic(music1);
        MusicPlayer.instance.PlayMusic();
    }
}
