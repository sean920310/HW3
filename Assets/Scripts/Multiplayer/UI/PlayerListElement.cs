using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListElement : MonoBehaviour
{
    [SerializeField] Button _giveHost;
    [SerializeField] TextMeshProUGUI _Name;
    bool _isMasterClient;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUp()
    {
        _Name.text = player.NickName;
        UpdateMasterClient();
    }

    public void UpdateMasterClient()
    {
        _isMasterClient = PhotonNetwork.IsMasterClient;
        _giveHost.gameObject.SetActive(_isMasterClient && !player.IsMasterClient);
    }

    public void OnGiveHostClick()
    {
        RoomManager.ChangeMasterClient(player);
    }
}
