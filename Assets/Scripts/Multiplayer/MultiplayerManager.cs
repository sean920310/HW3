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
        Debug.Log("player instantiate");
        PhotonManager.Instance.PlayerInstantiate(gameObject, info.photonView.IsMine);
    }

}
