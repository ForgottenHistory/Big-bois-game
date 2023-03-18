using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public GameObject LobbyMenu;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    
    void Start () {
        EnableMenu(MainMenu);
    }

    public void EnableMenu( GameObject menu ) {
        LobbyMenu.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
