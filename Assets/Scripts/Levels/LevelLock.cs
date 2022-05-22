using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//detects if a level has been completed or not
public class LevelLock : MonoBehaviour
{
    public LevelDataHolder dataHolder;
    public int index;
    public bool unlocked;
    Image image;
    void OnEnable()
    {
        image = GetComponent<Image>();
        if(!dataHolder.levelsData.levelsUnlocked[index-1]){
            image.color = new Color32(100, 100, 100, 255);
            unlocked = false;
            if(gameObject.GetComponent<Button>() != null){
                Button button = gameObject.GetComponent<Button>();
                Navigation nav = button.navigation;
                nav.mode = Navigation.Mode.None;
                button.navigation = nav;
            }
        }
        else{
            unlocked = true;
        }
    }
}
