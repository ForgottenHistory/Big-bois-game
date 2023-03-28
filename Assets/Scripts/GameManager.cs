using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using UnityEngine;

public class GameManager : NetworkBehaviour, IInitialize
{
    /////////////////////////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    /////////////////////////////////////////////////////////////////////////////////////

    public static GameManager Instance { get; private set; }

    public NetworkObject player;

    float startupTime = 2f;

    public Transform spawnPointsParent;

    public bool isActive { get; set; } = true;

    /////////////////////////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES 
    /////////////////////////////////////////////////////////////////////////////////////
    
    int nextSpawnIndex = 0;

    /////////////////////////////////////////////////////////////////////////////////////

    List<Vector3> spawnPoints = new List<Vector3>();
    List<PlayerController> players = new List<PlayerController>();

    ServerManager serverManager;

    /////////////////////////////////////////////////////////////////////////////////////


    public void Initialize()
    {
        /////////////////////////////////////////////////////////////////////////////////////

        if (spawnPointsParent != null)
        {
            foreach (Transform t in spawnPointsParent)
            {
                spawnPoints.Add(t.position);
            }
        }
        else
        {
            Debug.LogError("No spawn points parent set!");
        }

        /////////////////////////////////////////////////////////////////////////////////////

        serverManager = InstanceFinder.ServerManager;
        foreach (int playerID in serverManager.Clients.Keys)
        {
            NetworkConnection conn = serverManager.Clients[playerID];
            SpawnPlayerRpc(conn);
        }

        /////////////////////////////////////////////////////////////////////////////////////

        StartGameClientRPC();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void Deinitialize()
    {
        isActive = false;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator InitializeCoroutine()
    {
        yield return new WaitForSeconds(startupTime);
        Initialize();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(InitializeCoroutine());
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // 
    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStartClient()
    {
        base.OnStartClient();
        Instance = this;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    [Server]
    public void SpawnPlayerRpc(NetworkConnection conn)
    {
        NetworkObject p = Instantiate(player, spawnPoints[nextSpawnIndex], Quaternion.identity);
        ServerManager.Spawn(p, conn);

        //GameObject p = GameObject.Instantiate(player, spawnPoints[nextSpawnIndex], Quaternion.identity);
        //serverManager.Spawn(p, conn);
        
        p.GetComponent<PlayerController>().Initialize();
        players.Add(p.GetComponent<PlayerController>());

        nextSpawnIndex++;
        if(nextSpawnIndex >= spawnPoints.Count)
        {
            nextSpawnIndex = 0;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    [ObserversRpc]
    void StartGameClientRPC()
    {
        GameObject startupScreen = GameObject.Find("StartupScreen");
        if (startupScreen == null)
            Debug.LogError("No object with name StartupScreen found!");
        GameObject.Find("StartupScreen").SetActive(false);
    }

    /////////////////////////////////////////////////////////////////////////////////////
}
