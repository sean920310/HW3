using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// https://doc.photonengine.com/pun/current/lobby-and-matchmaking/matchmaking-and-lobby

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] RoomListUI roomListUI;
    [SerializeField] GameObject passwordFieldPrefab;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    // this is for join room
    static private string _joinRoomName;
    static private GameObject _selectedRoom;
    static private string _roomPWD = "";
    private GameObject tempPasswordField; 
    private bool ableToJoin; // "Room not require pwd" or "passsword correct" -> true

    public override void OnEnable()
    {
        base.OnEnable();
        passwordFieldPrefab.SetActive(false);
        updateList();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    public void updateList()
    {
        roomListUI.RoomListUpdate(cachedRoomList);
    }

    static public void updateSelectedRoom(GameObject roomElement)
    {
        _selectedRoom = roomElement;
        _joinRoomName = _selectedRoom.GetComponent<RoomElement>().RoomName;
        _roomPWD = _selectedRoom.GetComponent<RoomElement>().Password;
    }

    static public void updateJoinRoomName(string joinRoomName)
    {
        _joinRoomName = joinRoomName;
    }

    public void OnClickJoinRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Server Not Connect");
            return;
        }

        if (_roomPWD.Length > 0) // require password 
        {
            // show password field
            passwordFieldPrefab.SetActive(true);
            passwordFieldPrefab.GetComponent<PasswordSection>().roomList = this.gameObject;
        }
        else
        {
            //RoomList.updateJoinRoomName(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            PhotonNetwork.JoinRoom(_joinRoomName);
        }
    }

    public void pwdLeaveButtonClick()
    {
        passwordFieldPrefab.SetActive(false);
    }

    public bool pwdJoinButtonClick(string pwd)
    {
        if (pwd == _roomPWD) // passwrod correct
        {
            passwordFieldPrefab.SetActive(false);
            PhotonNetwork.JoinRoom(_joinRoomName);
            return true;
        }
        return false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Filed: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room successfully.");
        MultiplayerMenuManager.resetAndOpenState(MultiplayerMenuManager.MenuStates.Room);
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
        roomListUI.RoomListUpdate(cachedRoomList);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }
}
