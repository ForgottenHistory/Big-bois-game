using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class NetworkDataContainer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started");
        // Cant use this
        //DontDestroyOnLoad(gameObject);
    }
}
