using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerManager : MonoBehaviour ,IPunInstantiateMagicCallback
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("something instantiate");
        if (info.photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.instance.Player1 = gameObject;
            }
            else
            {
                GameManager.instance.Player2 = gameObject;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.instance.Player2 = gameObject;
            }
            else
            {
                GameManager.instance.Player1 = gameObject;
            }
        }
    }
}
