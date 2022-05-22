using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//reads the json file that holds the list of unlocked levels
public class LevelsJSONParser : MonoBehaviour
{
    public void SaveToJSON(int i){
        string json = File.ReadAllText(Application.dataPath + "/LevelsDataFile.json");
        LevelsData data = JsonUtility.FromJson<LevelsData>(json);

        data.levelsUnlocked[i] = true;
        string saveJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/LevelsDataFile.json", saveJson);
    }

    public LevelsData LoadFromJson(){
        string json = File.ReadAllText(Application.dataPath + "/LevelsDataFile.json");
        LevelsData data = JsonUtility.FromJson<LevelsData>(json);
        return data;
    }
}
