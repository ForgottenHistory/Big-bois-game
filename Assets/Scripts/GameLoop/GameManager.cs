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

    public TimeOfDayClock timeOfDayClock;

    public bool isActive { get; set; } = true;

    /////////////////////////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES 
    /////////////////////////////////////////////////////////////////////////////////////

    int nextSpawnIndex = 0;

    /////////////////////////////////////////////////////////////////////////////////////

    List<Vector3> spawnPoints = new List<Vector3>();
    List<PlayerController> players = new List<PlayerController>();

    ServerManager serverManager;
    CustomerManager customerManager;

    public float timeOfDaySpeed = 1f;
    public float customerSpawnRate = 1f;
    public float nextSpawnTime = 25f;

    public float GetTimeOfDay { get { return timeOfDay; } }

    float timeOfDay = 0f;
    int day = 1;
    float spawnTime = 25.0f;

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

        customerManager = GetComponent<CustomerManager>();
        customerManager.Initialize();
        customerManager.isActive = true;
        
        spawnTime = nextSpawnTime;
        timeOfDayClock.Initialize();

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

    [Server]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCustomer();
        }

        timeOfDay += Time.deltaTime * timeOfDaySpeed;
        UpdateSpawnrateWithTime();
    }

    void UpdateSpawnrateWithTime()
    {
        float spawnRate = customerSpawnRate;
        if (timeOfDay > 6 * 60 && timeOfDay <= 7 * 60) // 6 am to 7 am
        {
            spawnRate = customerSpawnRate * 1.2f;
        }
        else if (timeOfDay > 7 * 60 && timeOfDay <= 9 * 60) // 7 am to 9 am
        {
            spawnRate = customerSpawnRate * 1.2f;
        }
        else if (timeOfDay > 9 * 60 && timeOfDay <= 11 * 60) // 9 am to 11 am
        {
            spawnRate = customerSpawnRate;
        }
        else if (timeOfDay > 11 * 60 && timeOfDay <= 13 * 60) // 11 am to 1 pm
        {
            spawnRate = customerSpawnRate * 1.5f;
        }
        else if (timeOfDay > 13 * 60 && timeOfDay <= 17 * 60) // 1 pm to 5 pm
        {
            spawnRate = customerSpawnRate * 1.2f;
        }
        else if (timeOfDay > 17 * 60 && timeOfDay <= 21 * 60) // 5 pm to 9 pm
        {
            spawnRate = customerSpawnRate * 1.4f;
        }
        else if (timeOfDay > 21 * 60 && timeOfDay <= 24 * 60) // 9 pm to midnight
        {
            spawnRate = customerSpawnRate * 1.1f;
        }

        // Spawn customers at the appropriate rate
        if (Time.time >= spawnTime)
        {
            SpawnCustomer();
            spawnTime = Time.time + nextSpawnTime / spawnRate;
        }
    }

    void SpawnCustomer()
    {
        customerManager.SpawnCustomer();
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
        if (nextSpawnIndex >= spawnPoints.Count)
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
