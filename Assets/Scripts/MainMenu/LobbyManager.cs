using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using FishNet.Observing;
using FishNet.Managing.Server;
using FishNet.Managing.Scened;

public class LobbyManager : NetworkBehaviour
{
    List<NetworkConnection> connections = new List<NetworkConnection>();
    public MainMenuUI mainMenuUI;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
        mainMenuUI.UpdatePlayerListTxt(1);
    }

    public void SwitchScene( string sceneName ) {
        //Make scene data.
        SceneLoadData sld = new SceneLoadData( sceneName );
        sld.ReplaceScenes = ReplaceOption.All;

        LoadOptions loadOptions = new LoadOptions
        {
            AutomaticallyUnload = true,
        };

        sld.Options = loadOptions;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }
}
