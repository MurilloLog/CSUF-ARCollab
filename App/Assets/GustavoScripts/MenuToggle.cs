using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuToggle : MonoBehaviour
{
    bool currentState;
    [SerializeField] UnityEvent turnedOn;
    [SerializeField] UnityEvent turnedOff;
    public void ToggleState(){
        currentState = !currentState;
        if(currentState)
            turnOn();
        else
            turnOff();
    }

    public void turnOn(){
        currentState = true;
        turnedOn.Invoke();
    }

    public void turnOff(){
        currentState = false;
        turnedOff.Invoke();
    }
}
