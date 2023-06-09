using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    [SerializeField] private RectTransform messagePanel;
    [SerializeField] private RectTransform messageContent;
    [SerializeField] private RectTransform messageElementPrefeb;
    [SerializeField] private TMP_InputField chatInput;

    private PhotonView pv;
    private List<string> messageList;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        pv = GetComponent<PhotonView>();
        messageList = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(chatInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            print("SendMessage: " + chatInput.text);
            SendMessageToChat(chatInput.text);
            chatInput.text = null;
        }
        if (!chatInput.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            chatInput.ActivateInputField();
            messagePanel.gameObject.SetActive(true);
            chatInput.gameObject.SetActive(true);
        }
        if (/*chatInput.isFocused && */Input.GetKeyDown(KeyCode.Escape))
        {
            chatInput.DeactivateInputField();

            messagePanel.gameObject.SetActive(false);
            chatInput.gameObject.SetActive(false);
        }
    }

    public void SendMessageToChat(string message)
    {
        pv.RPC("RpcSendMessageToChat", RpcTarget.All, message);
    }

    public void UpdateChatText()
    {
        foreach (Transform child in messageContent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in messageList)
        {
            RectTransform tmpMsg = Instantiate(messageElementPrefeb);
            tmpMsg.gameObject.SetActive(true);
            tmpMsg.GetComponentInChildren<TextMeshProUGUI>().text = item.ToString();
            tmpMsg.SetParent(messageContent);
        }
    }


    #region PunRPC

    [PunRPC]
    void RpcSendMessageToChat(string message, PhotonMessageInfo info)
    {
        messageList.Add(info.Sender.NickName + " : " + message);
        if (messageList.Count > 100)
            messageList.RemoveAt(0);
        UpdateChatText();
    }

    #endregion


}
