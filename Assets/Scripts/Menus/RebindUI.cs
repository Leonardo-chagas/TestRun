using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

//functions for the rebinding buttons of each action
public class RebindUI : MonoBehaviour
{
    public InputActionReference inputActionReference;
    public bool excludeMouse = true;
    [Range(0, 10)]
    public int selectedBinding;
    public InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Info - DO NOT EDIT")]
    public InputBinding inputBinding;
    public int bindingIndex;
    private string actionName;

    [Header("UI Fields")]
    //public TMP_Text actionText;
    public Button rebindButton;
    public TMP_Text rebindText;
    public Button resetButton;
    public int compositeIndex;

    private void OnEnable(){
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if(inputActionReference != null){
            BindingManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }

        BindingManager.rebindComplete += UpdateUI;
        BindingManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable(){
        BindingManager.rebindComplete -= UpdateUI;
        BindingManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate(){
        if(inputActionReference == null) return;
        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo(){
        if(inputActionReference.action != null)
            actionName = inputActionReference.action.name;

        if(inputActionReference.action.bindings.Count > selectedBinding){
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI(){
        //if(actionText != null)
            //actionText.text = actionName;
        
        if(rebindText != null){
            if(Application.isPlaying){
                rebindText.text = BindingManager.GetBindingName(actionName, bindingIndex);
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind(){
        BindingManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse);
    }

    private void ResetBinding(){
        BindingManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}
