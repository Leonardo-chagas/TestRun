using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

//marks a level as true on the json file and loads next scene
public class LevelComplete : MonoBehaviour
{
    public int index;
    public Animator fade;
    
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            LevelDataHolder.instance.Save(index);
            col.transform.position = new Vector3(col.transform.position.x+1, col.transform.position.y,
             col.transform.position.z);
            Destroy(col.gameObject.GetComponent<Player1>());
            Destroy(col.gameObject.GetComponent<PlayerInput>());
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            fade.SetBool("fade", true);
        }
    }
}
