using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _roomName;
    [SerializeField] TextMeshProUGUI _password;
    public string RoomName { get => _roomName.text; }
    public string Password { get => _password.text; }


    public void RoomSelected()
    {
        RoomList.updateSelectedRoom(this.gameObject);
    }
}
