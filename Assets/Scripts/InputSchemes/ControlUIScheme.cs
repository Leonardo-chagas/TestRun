using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds the button sprites for each control scheme
[CreateAssetMenu(fileName="Data", menuName="ScriptableObjects/ControlUIScheme", order=1)]
public class ControlUIScheme : ScriptableObject
{
    public GameObject[] PlaystationButtons;
    public GameObject[] XboxButtons;
    public GameObject[] PCButtons;
}
