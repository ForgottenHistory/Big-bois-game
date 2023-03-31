using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class TestMovement : NetworkBehaviour
{
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.up * Time.deltaTime * 5f;
            }
        }
    }
}
