using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the line between the smasher and its base
public class SmasherLine : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    
    LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        UpdateLine();
    }

    void UpdateLine(){
        line.SetPosition(1, startPoint.position);
        line.SetPosition(0, endPoint.position);
    }

}
