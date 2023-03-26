using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;
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
    // START SERVER/ JOIN AS CLIENT
    /////////////////////////////////////////////////////////////////////////////////////

    public async void CreateServer()
    {
        ChangeIpOnTugboat("localhost");
        bool result = InstanceFinder.ServerManager.StartConnection();
        Debug.Log("Server started: " + result);

        if (result == false)
        {
            Debug.LogError("Server failed to start.");
            return;
        }

        await Task.Delay(500); // Wait to avoid race condition
        InstanceFinder.ClientManager.StartConnection("localhost");
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void JoinLobby()
    {

        ChangeIpOnTugboat(IPInputField.text);
        InstanceFinder.ClientManager.StartConnection(IPInputField.text);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // LOBBY FUNCTIONS
    /////////////////////////////////////////////////////////////////////////////////////

    public void SwitchToLobby()
    {
        ConnectInfoMenu.SetActive(false);
        LobbyInfoMenu.SetActive(true);

        if( InstanceFinder.IsHost){
            startBtn.SetActive(true);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void DisconnectFromLobby(){
        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.StopConnection();
        if (InstanceFinder.IsServer)
            InstanceFinder.ServerManager.StopConnection(false);
        EnableMenu(MainMenu);
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
    // DISCONNECT / QUIT GAME
    /////////////////////////////////////////////////////////////////////////////////////

    public void QuitGame()
    {
        Application.Quit();
    }
    
    /////////////////////////////////////////////////////////////////////////////////////
}
