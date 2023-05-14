using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] RectTransform roomList;
    [SerializeField] GameObject roomElementPrefab;

    private GameObject roomElementPrefabTemp;

    public void RoomListUpdate(Dictionary<string, RoomInfo> roomDict)
    {

        if (!PhotonNetwork.IsConnected) return;

        foreach (Transform child in roomList.transform)
        {
            if(child != roomList.transform.GetChild(0))
                Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, RoomInfo> room in roomDict)
        {
            if (room.Value.PlayerCount >= room.Value.MaxPlayers)
                continue;

            roomElementPrefabTemp = Instantiate(roomElementPrefab);

            roomElementPrefabTemp.transform.parent = roomList.transform;

            roomElementPrefabTemp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Value.Name.ToString(); // room name
            roomElementPrefabTemp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.Value.PlayerCount.ToString() + " / " + room.Value.MaxPlayers.ToString(); // room name

            string roomPWD = (string)room.Value.CustomProperties["pwd"];
            if (roomPWD != null && roomPWD.Length > 0)
            {
                roomElementPrefabTemp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Yes";
                roomElementPrefabTemp.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = room.Value.CustomProperties["pwd"].ToString();
            }
            else 
                roomElementPrefabTemp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "No"; // password need


            roomElementPrefabTemp.SetActive(true);
        }
    }
}
