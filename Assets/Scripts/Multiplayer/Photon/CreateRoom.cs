using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    // this is for create room
    private string _roomName;
    private string _password;

    private Int32 _maxPlayer = 2;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public void roomNameUpdate(string roonName)
    {
        _roomName = roonName;
    }

    public void passwordUpdate(string password)
    {
        _password = password;
    }

    public void maxPlayerUpdate(Int32 maxPlayer)
    {
        _maxPlayer = maxPlayer + 2;
    }

    public void OnClickCreateRoom()
    {

        if (!PhotonNetwork.IsConnected) {
            Debug.Log("Server Not Connect");
            return;
        }

        RoomOptions roomSetting = new RoomOptions();
        roomSetting.MaxPlayers = ((byte)_maxPlayer);
        roomSetting.IsVisible = true;
        roomSetting.IsOpen = true;
        roomSetting.CustomRoomProperties = new Hashtable() { { "pwd", _password } };
        roomSetting.CustomRoomPropertiesForLobby =  new string[] { "pwd" };

        PhotonNetwork.CreateRoom(_roomName, roomSetting, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room successfully.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Filed: " + message);

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Filed: " + message);
    }

}
