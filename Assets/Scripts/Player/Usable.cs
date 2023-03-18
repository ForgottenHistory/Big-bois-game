using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    //usable base script

    public bool hold;
    public Usable(bool hold)
    {
        this.hold = hold;
    }
    public abstract void Use();
}