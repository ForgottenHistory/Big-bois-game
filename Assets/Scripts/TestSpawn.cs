using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;

public class TestSpawn : NetworkBehaviour
{
    public NetworkObject spawnPrefab;
    public Transform spawnPointsParent;
    int nextSpawnIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Spawn( LocalConnection );
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void Spawn( NetworkConnection conn )
    {
        Debug.Log("Spawned");
        NetworkObject p = Instantiate(spawnPrefab, spawnPointsParent.GetChild(nextSpawnIndex).position, Quaternion.identity);
        ServerManager.Spawn(p, conn );

        nextSpawnIndex++;
        if (nextSpawnIndex >= spawnPointsParent.childCount)
            nextSpawnIndex = 0;
    }
}
