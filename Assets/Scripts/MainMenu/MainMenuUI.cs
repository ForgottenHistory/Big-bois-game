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

public class MainMenuUI : MonoBehaviour
{
    public GameObject LobbyMenu;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject ConnectInfoMenu;
    public GameObject LobbyInfoMenu;

    public TMP_InputField IPInputField;
    public TMP_InputField PortInputField;

    public TMP_Text playerListTxt;

    int playerCount = 0;

    void Start()
    {

        EnableMenu(MainMenu);
        Tugboat tugboatTransport = InstanceFinder.NetworkManager.GetComponent<Tugboat>();
        playerListTxt.text = "";
    }

    public void EnableMenu(GameObject menu)
    {

        LobbyMenu.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        menu.SetActive(true);

        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.StopConnection();
        else if (InstanceFinder.IsServer)
            InstanceFinder.ServerManager.StopConnection(false);
    }

    public void ChangeIpOnTugboat(string ip)
    {

        Tugboat tugboatTransport = InstanceFinder.NetworkManager.GetComponent<Tugboat>();
        tugboatTransport.SetClientAddress(ip);
    }

    public void JoinLobby()
    {

        ChangeIpOnTugboat(IPInputField.text);
        InstanceFinder.ClientManager.StartConnection(IPInputField.text);
    }

    public void CreateServer()
    {

        ChangeIpOnTugboat("localhost");
        bool result = InstanceFinder.ServerManager.StartConnection();
        Debug.Log("Server started: " + result);

        if (result == false)
        {
            Debug.LogError("Server failed to start.");
            return;
        }

        CreateLobby();
    }

    void CreateLobby()
    {
        ConnectInfoMenu.SetActive(false);
        LobbyInfoMenu.SetActive(true);
    }

    public void UpdatePlayerListTxt(int newPlayer)
    {
        playerCount += newPlayer;
        for (int i = 0; i < playerCount; i++)
        {
            playerListTxt.text += "Player " + (i+1) + "\n";
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
