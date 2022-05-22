using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//Detects if there has been a change on the input type
public class InputManager : MonoBehaviour
{

    enum CurrentControllerType {Playstation, Xbox, Other};
    CurrentControllerType controller;
    CurrentControlScheme controlScheme;
    
    void Start()
    {
        InputSystem.onDeviceChange += InputDeviceChanged;
        controlScheme = GetComponent<CurrentControlScheme>();
    }

    public void InputDeviceChanged(InputDevice device, InputDeviceChange change){
        switch(change){

            case InputDeviceChange.Added:
                Debug.Log("New device added");

                if(device.description.manufacturer == "Sony Interactive Entertainment" &&
                controller != CurrentControllerType.Playstation){
                    Debug.Log("Playstation Controller detected");
                    controlScheme.ChangeToPlaystationControls();
                    controller = CurrentControllerType.Playstation;
                }
                else if(device.description.manufacturer != "Sony Interactive Entertainment" &&
                controller != CurrentControllerType.Xbox){
                    Debug.Log("Xbox Controller detected");
                    controlScheme.ChangeToXboxControls();
                    controller = CurrentControllerType.Xbox;
                }
                break;

            case InputDeviceChange.Disconnected:
                //controllerDisconnected.Invoke();
                Debug.Log("Device disconnected");
                controller = CurrentControllerType.Other;
                break;

            case InputDeviceChange.Reconnected:
                //controllerReconnected.Invoke();
                Debug.Log("Device reconnected");

                if(device.description.manufacturer == "Sony Interactive Entertainment" &&
                controller != CurrentControllerType.Playstation){
                    Debug.Log("Playstation Controller detected");
                    controlScheme.ChangeToPlaystationControls();
                    controller = CurrentControllerType.Playstation;
                }
                else if(device.description.manufacturer != "Sony Interactive Entertainment" &&
                controller != CurrentControllerType.Xbox){
                    Debug.Log("Xbox Controller detected");
                    controlScheme.ChangeToXboxControls();
                    controller = CurrentControllerType.Xbox;
                }
                break;
        
            default:
                break;
        }
    }
}
