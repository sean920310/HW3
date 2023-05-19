using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MultiplayerMenuManager : MonoBehaviour, ISettingsDataPersistence
{
    [Serializable]
    // Menu State Enum
    public enum MenuStates
    {
        None = 0,
        SetName  = 1 << 0,
        Multiplayer  = 1 << 1,
        RoomList     = 1 << 2,
        RoomCreate   = 1 << 3,
        Room         = 1 << 4,
        Settings     = 1 << 5,
    }

    // Menu OBJ
    [SerializeField] private GameObject SetNamePanel;
    [SerializeField] private GameObject MultiplayerPanel;
    [SerializeField] private GameObject RoomListPanel;
    [SerializeField] private GameObject RoomCreatePanel;
    [SerializeField] private GameObject RoomPanel;
    [SerializeField] private GameObject SettingsPanel;

    [SerializeField] private LoadingScene loadingScene;

    [SerializeField] private const MenuStates defaultState = MenuStates.Multiplayer;
    public static uint menuState = ((uint)defaultState); // Multiplayer Panel 

    private string playername;


    // Update is called once per frame
    void Update()
    {
        // Active Different Menu by getting menu state
        SetNamePanel.SetActive(getState(MenuStates.SetName));
        MultiplayerPanel.SetActive(getState(MenuStates.Multiplayer));
        RoomListPanel.SetActive(getState(MenuStates.RoomList));
        RoomCreatePanel.SetActive(getState(MenuStates.RoomCreate));
        RoomPanel.SetActive(getState(MenuStates.Room));
        SettingsPanel.SetActive(getState(MenuStates.Settings));
    }

    private void OnDestroy()
    {
        menuState = ((uint)defaultState);
    }

    /// <summary>
    /// Using this function to switch between different menus.
    /// </summary>
    public static void setState(MenuStates state, bool isOpen)
    {
        menuState = isOpen ? (menuState | (uint)state) : (menuState & (~((uint)state)));
    }

    /// <summary>
    /// Using this function to check whether a particular menu is open or not.
    /// </summary>
    public static bool getState(MenuStates state)
    {
        return ((menuState & (uint)state) != 0);
    }

    /// <summary>
    /// Use this function to reset the current menu state and set it to other state.
    /// </summary>
    public static void resetAndOpenState(MenuStates state)
    {
        menuState = 0;
        menuState = (menuState | (uint)state);
    }

    /// <summary>
    /// Use this function to reset the current menu state and set it to other state by string, this function is for OnClick().
    /// <br/>
    /// reference: https://answers.unity.com/questions/1549639/enum-as-a-function-param-in-a-button-onclick.html
    /// </summary>
    public static void resetStateByString(string stateString)
    {
        resetAndOpenState((MenuStates)Enum.Parse(typeof(MenuStates), stateString));
    }

    public void playerNameUpdate()
    {
        Photon.Pun.PhotonNetwork.NickName = playername;
    }

    public void onNameSetSubmit()
    {
        playername = SetNamePanel.GetComponentInChildren<TMP_InputField>().text;

        if (GameSettingsManager.instance)
            GameSettingsManager.instance.settingsData.multiplayerName = playername;

        SettingsPanel.GetComponentInChildren<MultiplayerSettings>().playerNameInput.text = playername;

        playerNameUpdate();
        resetAndOpenState(defaultState);
    }

    public void onQuit()
    {
        loadingScene.LoadScene(0);
    }

    public void LoadData(GameSettingsData data)
    {
        playername = data.multiplayerName;

        if (playername == "")
        {
            resetAndOpenState(MenuStates.SetName);
        }

        playerNameUpdate();
    }

    public void SaveData(GameSettingsData data)
    {
        data.multiplayerName = Photon.Pun.PhotonNetwork.NickName;
    }
}
