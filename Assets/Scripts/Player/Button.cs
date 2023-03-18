using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Usable
{
    //customizable button

    public UnityEvent invokeMethod; //method set in editor

    public Button(bool hold) : base(hold)
    {

    }

    public override void Use()
    {
        invokeMethod.Invoke();
    }
}