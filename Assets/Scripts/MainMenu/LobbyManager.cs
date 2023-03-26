using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using System.Linq;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////////
    // Summary: This script is a network extension of MainMenuUI.
    // Designed to hold player connection information and manage scene loading.
    // Reason to be on a separate object is to not have NetworkObj on Canvas
    /////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////
    // STRUCT FOR PLAYER INFO
    /////////////////////////////////////////////////////////////////////////////////////

    public struct PlayerContainer
    {
        public string PlayerName;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public MainMenuUI mainMenuUI;

    [SyncObject]
    readonly SyncDictionary<int, PlayerContainer> playerContainers = new();

    /////////////////////////////////////////////////////////////////////////////////////
    //
    //                                  FUNCTIONS
    //
    /////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////
    // START SERVER/CLIENT
    /////////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        playerContainers.OnChange += PlayerContainers_OnChange;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Debug.Log("Server started");
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Debug.Log("Client started");

        mainMenuUI.SwitchToLobby();
        ServerAddPlayer(LocalConnection.ClientId, mainMenuUI.PlayerNameInputField.text);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // ADD PLAYERCONTAINER TO DICTIONARY
    /////////////////////////////////////////////////////////////////////////////////////

    // Sent to server from client
    [ServerRpc(RequireOwnership = false)]
    public void ServerAddPlayer(int id, string playerName)
    {
        PlayerContainer pc = new PlayerContainer();
        pc.PlayerName = playerName;
        playerContainers.Add(id, pc);
    }

    /////////////////////////////////////////////////////////////////////////////////////

    // Sent to all clients
    [ObserversRpc]
    void UpdatePlayerListTxtRpc()
    {
        if (InstanceFinder.ServerManager.isActiveAndEnabled == false)
        {
            Debug.LogWarning("Server is not active. Cannot update the player list.");
            return;
        }

        mainMenuUI.UpdatePlayerListTxt(playerContainers.Values.Select(pc => pc.PlayerName).ToList());
    }

    /////////////////////////////////////////////////////////////////////////////////////

    private void PlayerContainers_OnChange(SyncDictionaryOperation op, int key, PlayerContainer value, bool asServer)
    {
        switch (op)
        {
            case SyncDictionaryOperation.Add:
                UpdatePlayerListTxtRpc();
                break;
            case SyncDictionaryOperation.Remove:
                UpdatePlayerListTxtRpc();
                break;
            case SyncDictionaryOperation.Set:
                UpdatePlayerListTxtRpc();
                break;
            case SyncDictionaryOperation.Clear:
                UpdatePlayerListTxtRpc();
                break;
            case SyncDictionaryOperation.Complete:
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // SCENE MANAGEMENT
    /////////////////////////////////////////////////////////////////////////////////////

    [Server]
    public void SwitchScene(string sceneName)
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        sld.ReplaceScenes = ReplaceOption.All;

        LoadOptions loadOptions = new LoadOptions
        {
            AutomaticallyUnload = true,
            AllowStacking = false,
        };

        //sld.Options = loadOptions;
        SceneManager.LoadGlobalScenes(sld);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // STOP SERVER / CLIENT
    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStopServer()
    {
        base.OnStopServer();
        playerContainers.Clear();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public override void OnStopClient()
    {
        base.OnStopClient();
        // This is called when object is deinitialized
        // It needs to be called when a disconnect happens
        //ServerRemovePlayer(LocalConnection.ClientId);
        //mainMenuUI.DisconnectFromLobby();
        //playerContainers.Clear();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    [ServerRpc(RequireOwnership = false)]
    public void ServerRemovePlayer(int id)
    {
        playerContainers.Remove(id);
        //UpdatePlayerListTxtRpc();
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // DEBUG
    /////////////////////////////////////////////////////////////////////////////////////

    [ServerRpc(RequireOwnership = false)]
    private void RpcSendChat(string msg, NetworkConnection conn = null)
    {
        Debug.Log($"Received {msg} on the server from connection {conn.ClientId}.");
    }

    /////////////////////////////////////////////////////////////////////////////////////
}
