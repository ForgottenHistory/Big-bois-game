using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class NetworkDataContainer : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        //Debug.Log("Client started");
        // Cant use this
        //DontDestroyOnLoad(gameObject);
    }
}
