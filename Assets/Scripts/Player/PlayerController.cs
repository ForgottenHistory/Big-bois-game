using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;

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

    public int owner = 0;

    public bool isActive { get; set; } = false;
    public bool isDebug { get; set; } = false;

    /////////////////////////////////////////////////////////////////////////////////////
    // START
    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Debug.Log("IsOwner: " + IsOwner + " IsServer: " + NetworkManager.IsServer + " ClientId" + LocalConnection.ClientId);
        if( IsOwner == true )
            Initialize();
        else
            Deinitialize();
    }

    public void Initialize()
    {   
        if (IsOwner == false)
        {
            Debug.Log("Deinitialize");
            return;
        }

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
        if( isActive == false || IsOwner == false )
            return;

        Movement();
        ExitGame();

        // Debug
        SwitchDebugMode();
        if( isDebug == true)
            DebugMode();
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

    /////////////////////////////////////////////////////////////////////////////////////
    // MISC
    /////////////////////////////////////////////////////////////////////////////////////

    void ExitGame()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
            Application.Quit();
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // DEBUG
    /////////////////////////////////////////////////////////////////////////////////////

    public void SwitchDebugMode() {
        if( Input.GetKeyDown(KeyCode.F1) )
            isDebug = !isDebug;
    }

    void DebugMode() {

        // RPC Tests
        if( Input.GetKeyDown(KeyCode.F2) ) // Server to clients
            DebugMessageRpc("ObserverRPC Test");
        else if( Input.GetKeyDown(KeyCode.F3) ) // Client to server to clients
            DebugMessageServerRpc("ServerRPC Test with no ownership");
        else if( Input.GetKeyDown(KeyCode.F4) ) // Client to server to clients
            DebugMessageServerRpcNoOwnership("ServerRPC Test with ownership");
        else if( Input.GetKeyDown(KeyCode.F5) ) // Client to server
            DebugMessageServerRpcServerOnly("ServerRPC Test with server only");
    }

    // Changes made from client to server will be on the server only
    [ServerRpc]
    public void DebugMessageServerRpcServerOnly(string message)
    {
        // Unless the object is synced through network behaviour this change will only be on the server
        TMPro.TMP_Text text = GameObject.Find("TestText").GetComponent<TMPro.TMP_Text>();
        text.text = message;
    }

    // This will called on the server to clients with no ownership needed of the calling object
    [ServerRpc]
    public void DebugMessageServerRpcNoOwnership(string message)
    {
        DebugMessageRpc(message);
    }

    // This will called on the server to clients with ownership needed of the calling object
    [ServerRpc(RequireOwnership = false)]
    public void DebugMessageServerRpc(string message)
    {
        DebugMessageRpc(message);
    }

    // From host/server to clients
    [ObserversRpc]
    public void DebugMessageRpc(string message)
    {
        TMPro.TMP_Text text = GameObject.Find("TestText").GetComponent<TMPro.TMP_Text>();
        text.text = message;
    }
}
