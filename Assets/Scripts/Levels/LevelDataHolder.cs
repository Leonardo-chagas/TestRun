using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds the list of completed levels
public class LevelDataHolder : MonoBehaviour
{
    public LevelsData levelsData;
    LevelsJSONParser json;
    [HideInInspector] public static LevelDataHolder instance;
    void Start()
    {
        json = GetComponent<LevelsJSONParser>();
        levelsData = json.LoadFromJson();
        instance = this;
    }

    public void Save(int i){
        json.SaveToJSON(i);
        levelsData = json.LoadFromJson();
    }
}
