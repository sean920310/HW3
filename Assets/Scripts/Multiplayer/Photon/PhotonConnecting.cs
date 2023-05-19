using Photon.Pun; // important
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for connection test

// inherite "MonoBehaviourPunCallbacks" when using Photon Callback
public class PhotonConnecting : MonoBehaviourPunCallbacks
{
    public string version = "1.0";
    public void ConnectingToPhoton()
    {
        print("connecting");
        // connect to Photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    // Start is called before the first frame update
    public void Start()
    {
        // connection information setup
        PhotonNetwork.GameVersion = version; // Photon Server can create partitions for different versions

        PhotonNetwork.SendRate = 50; // How many informations server send.
        PhotonNetwork.SerializationRate = 40;
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.NickName = "Random" + Random.Range(0,999).ToString("0000");

        ConnectingToPhoton();
    }

    // call back when connected to server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to: " + PhotonNetwork.CloudRegion + " server.");
        PhotonNetwork.JoinLobby();
    }

    // call back when disconnected
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect: " + cause.ToString());
    }
}
