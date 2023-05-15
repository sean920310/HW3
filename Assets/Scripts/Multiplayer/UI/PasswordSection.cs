using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordSection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI information;
    [SerializeField] TMP_InputField inputPWD;
    public GameObject roomList;
    private string _inputPWD;

    public void Update()
    {
        //if(roomList == null) 
        //    Destroy(gameObject);
    }
    private void OnEnable()
    {
        information.text = "";
        inputPWD.text = "";
    }

    public void onPWDUpdate(string pwd)
    {
        _inputPWD = pwd;
    }

    public void onJoinBTNClick()
    {
        bool isPwdCorrect = roomList.GetComponent<RoomList>().pwdJoinButtonClick(_inputPWD);
        if(!isPwdCorrect)
        {
            information.text = "Password incorrect";
        }
        else
        {
            onLeaveBTNClick();
        }
    }

    public void onLeaveBTNClick()
    {
        roomList.GetComponent<RoomList>().pwdLeaveButtonClick();
    }
}
