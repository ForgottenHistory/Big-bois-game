using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class TestScript : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += ActivateObject;
    }

    void ActivateObject(SceneLoadEndEventArgs args ) {
        Debug.Log("Loaded");
        obj.SetActive(true);
    }
}
