using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class SceneSwitchTest : NetworkBehaviour
{
    void Update(){
        if( Input.GetKeyDown(KeyCode.Space) && IsHost )
            SwitchScene();
    }

    public void SwitchScene(){
        SceneLoadData sld = new SceneLoadData("Restaurant");
        sld.ReplaceScenes = ReplaceOption.All;

        LoadOptions loadOptions = new LoadOptions
        {
            AutomaticallyUnload = true,
            AllowStacking = false,
        };

        //sld.Options = loadOptions;
        SceneManager.LoadGlobalScenes(sld);
    }
}
