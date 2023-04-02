using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IUsable
{
    //customizable button

    public UnityEvent invokeMethod; //method set in editor

    public bool hold { get; } = false;

    public void Use()
    {
        invokeMethod.Invoke();
    }
}