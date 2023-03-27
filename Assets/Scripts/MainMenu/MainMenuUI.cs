using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using FishNet.Object.Synchronizing;
using System.Threading.Tasks;

public class MainMenuUI : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    /////////////////////////////////////////////////////////////////////////////////////

    public GameObject LobbyMenu;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject ConnectInfoMenu;
    public GameObject LobbyInfoMenu;

    public TMP_InputField IPInputField;
    public TMP_InputField PortInputField;
    public TMP_InputField PlayerNameInputField;

    public TMP_Text playerListTxt;

    public GameObject startBtn;

    /////////////////////////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES
    /////////////////////////////////////////////////////////////////////////////////////

    private NetworkManager networkManager;
    private LocalConnectionState clientState = LocalConnectionState.Stopped;
    private LocalConnectionState serverState = LocalConnectionState.Stopped;

    /////////////////////////////////////////////////////////////////////////////////////
    //
    //                                  FUNCTIONS
    //
    /////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////
    // START 
    /////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        EnableMenu(MainMenu);
        Tugboat tugboatTransport = InstanceFinder.NetworkManager.GetComponent<Tugboat>();
        playerListTxt.text = "";

        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager not found, BasicNetworkManager will not function.");
        }

        networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
    }

    private void OnDestroy()
    {
        if (networkManager == null)
            return;

        networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void EnableMenu(GameObject menu)
    {
        LobbyMenu.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        menu.SetActive(true);
        ConnectInfoMenu.SetActive(true);
        LobbyInfoMenu.SetActive(false);

        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.StopConnection();
        if (InstanceFinder.IsServer)
            InstanceFinder.ServerManager.StopConnection(false);
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void ChangeIpOnTugboat(string ip)
    {
        Tugboat tugboatTransport = InstanceFinder.NetworkManager.GetComponent<Tugboat>();
        tugboatTransport.SetClientAddress(ip);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // LOBBY FUNCTIONS
    /////////////////////////////////////////////////////////////////////////////////////

    public void SwitchToLobby()
    {
        ConnectInfoMenu.SetActive(false);
        LobbyInfoMenu.SetActive(true);

        Debug.Log("IsHost: " + InstanceFinder.IsServer);
        if (InstanceFinder.IsServer)
        {
            startBtn.SetActive(true);
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////
    // LOBBY INFO FUNCTIONS
    /////////////////////////////////////////////////////////////////////////////////////

    public void UpdatePlayerListTxt(List<string> playerList)
    {
        playerListTxt.text = "";
        foreach (string player in playerList)
        {
            playerListTxt.text += player + "\n";
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // START HOST / LOBBY
    /////////////////////////////////////////////////////////////////////////////////////

    public async void StartLobby()
    {
        if (networkManager == null)
            return;

        if (serverState != LocalConnectionState.Stopped)
        {
            Debug.LogWarning("Server is already started.");
            return;
        }
        networkManager.ServerManager.StartConnection();
        await Task.Delay(500); // Wait to avoid race condition
        
        StartClient();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void StopServer()
    {
        if (networkManager == null)
            return;

        if (serverState != LocalConnectionState.Stopped)
        {
            networkManager.ServerManager.StopConnection(true);
        }
        else
        {
            Debug.LogWarning("Server is not started.");
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // JOIN AS CLIENT
    /////////////////////////////////////////////////////////////////////////////////////

    public void StartClient()
    {
        if (networkManager == null)
            return;

        if (clientState == LocalConnectionState.Stopped)
        {
            networkManager.ClientManager.StartConnection();
            SwitchToLobby();
        }
        else
        {
            Debug.LogWarning("Client is already started.");
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void StopClient()
    {
        if (networkManager == null)
            return;

        if (clientState != LocalConnectionState.Stopped)
        {
            networkManager.ClientManager.StopConnection();
        }
        else
        {
            Debug.LogWarning("Client is not started.");
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // EVENTS
    /////////////////////////////////////////////////////////////////////////////////////

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        clientState = obj.ConnectionState;
    }


    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        serverState = obj.ConnectionState;
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // DISCONNECT / QUIT GAME
    /////////////////////////////////////////////////////////////////////////////////////

    public void QuitGame()
    {
        Application.Quit();
    }

    /////////////////////////////////////////////////////////////////////////////////////
}
