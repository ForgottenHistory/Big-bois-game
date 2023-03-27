using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour, IInitialize
{
    /////////////////////////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    /////////////////////////////////////////////////////////////////////////////////////

    public float speed = 5f;

    /////////////////////////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES
    /////////////////////////////////////////////////////////////////////////////////////

    private float gravity = -9.81f * 100.0f;
    private CharacterController characterController;
    private Vector3 velocity = Vector3.zero;

    public bool isActive { get; set; } = false;
    /////////////////////////////////////////////////////////////////////////////////////
    // START
    /////////////////////////////////////////////////////////////////////////////////////

    [ObserversRpc]
    public void Initialize()
    {  
        if (IsOwner != true)
        {
            Debug.Log("Deinitialize");
            Deinitialize();
            return;
        }
        Debug.Log("IsOwner: " + IsOwner );
        Debug.Log( "ClientId" + LocalConnection.ClientId);
        characterController = GetComponent<CharacterController>();
        GameObject cameraObj = transform.GetChild(0).gameObject;
        cameraObj.GetComponent<CameraController>().Initialize();
        cameraObj.GetComponent<PlayerUsable>().Initialize();
        isActive = true;
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // DEINITIALIZE
    /////////////////////////////////////////////////////////////////////////////////////

    public void Deinitialize()
    {
        GameObject cameraObj = transform.GetChild(0).gameObject;
        // cameraObj.GetComponent<PlayerUsable>().Deinitialize();
        // cameraObj.GetComponent<AudioListener>().enabled = false;
        // cameraObj.SetActive(false);
        Destroy(cameraObj);
        isActive = false;
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // UPDATE
    /////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {   
        if( isActive == false )
            return;

        if (IsOwner)
        {
            Movement();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // MOVEMENT
    /////////////////////////////////////////////////////////////////////////////////////

    void Movement()
    {
        velocity = Vector3.zero; // Reset velocity each frame

        if (Input.GetKey(KeyCode.W))
        {
            velocity += transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity -= transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity -= transform.right * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += transform.right * speed;
        }

        // Apply gravity
        if (characterController.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
