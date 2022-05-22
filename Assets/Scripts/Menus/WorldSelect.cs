using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//functions for selecting and moving the worlds
//also changes the colors of objects that cant be selected
public class WorldSelect : MonoBehaviour
{
    public World[] worlds;
    public Image rightArrow;
    public Image leftArrow;
    public Image select;
    public Button[] buttons;
    public GameObject[] levels;

    void Start(){
        leftArrow.color = new Color32(100, 100, 100, 255);
    }
    public void Select(){
        if(worlds[0].index == 2){
            levels[0].SetActive(true);
            gameObject.SetActive(false);
            buttons[0].Select();
        }
        else if(worlds[1].index == 2 && worlds[1].isUnlocked){
            levels[1].SetActive(true);
            gameObject.SetActive(false);
            //buttons[1].Select();
        }
        else if(worlds[2].index == 2 && worlds[2].isUnlocked){
            levels[2].SetActive(true);
            gameObject.SetActive(false);
            //buttons[2].Select();
        }
    }

    public void Right(){
        if(worlds[0].index == 0) return;
        foreach(World world in worlds){
            world.MoveLeft();
            if(world.index == 2 && !world.isUnlocked)
                select.color = new Color32(100, 100, 100, 255);
            else if(world.index == 2 && world.isUnlocked)
                select.color = new Color32(255, 255, 255, 255);
        }
        if(worlds[0].index == 0)
            rightArrow.color = new Color32(100, 100, 100, 255);
        else if(worlds[2].index != 4)
            leftArrow.color = new Color32(255, 255, 255, 255);
    }

    public void Left(){
        if(worlds[2].index == 4) return;
        foreach(World world in worlds){
            world.MoveRight();
            if(world.index == 2 && !world.isUnlocked)
                select.color = new Color32(100, 100, 100, 255);
            else if(world.index == 2 && world.isUnlocked)
                select.color = new Color32(255, 255, 255, 255);
        }
        if(worlds[2].index == 4)
            leftArrow.color = new Color32(100, 100, 100, 255);
        if(worlds[0].index != 0)
            rightArrow.color = new Color32(255, 255, 255, 255);
    }
}
