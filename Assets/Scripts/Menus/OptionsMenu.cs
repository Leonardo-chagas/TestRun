using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//function for the options menu
public class OptionsMenu : MonoBehaviour
{   
    //public ChangeSound sound;
    public Slider musicVolume, soundVolume, screenShakePower;
    public Toggle fullscreenToggle, vsyncToggle;

    void Start(){
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundVolume.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        screenShakePower.value = PlayerPrefs.GetFloat("ScreenShakePower", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync") == 1 ? true : false;
    }

    public void SetMusicVolume(){
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        MusicPlayer.instance.audioSource.volume = musicVolume.value;
    }

    public void SetSoundVolume(){
        PlayerPrefs.SetFloat("SoundVolume", soundVolume.value);
        //sound.ChangeSoundVolume();
        AudioListener.volume = soundVolume.value;
        MusicPlayer.instance.audioSource.volume = musicVolume.value;
    }

    public void SetScreenShakePower(){
        PlayerPrefs.SetFloat("ScreenShakePower", screenShakePower.value);
    }

    public void SetFullscreen(){
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        if(fullscreenToggle.isOn)
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        else
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
    }

    public void SetVSync(){
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync");
    }
}
