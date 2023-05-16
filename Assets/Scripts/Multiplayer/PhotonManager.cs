using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	static public PhotonManager Instance;

	[Header("Player")]
	[SerializeField] private GameObject playerPrefab;

	[Header("Ball")]
	[SerializeField] private GameObject ballPrefab;
	
	[SerializeField] StatesPanel p1StatesPanel;
	[SerializeField] StatesPanel p2StatesPanel;
	[SerializeField] Transform centerBorder;

	private GameObject myPlayer;
	private GameObject ballObject;
	private PhotonView pv;


	void Start()
	{
		Instance = this;

		pv = GetComponent<PhotonView>();

		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene(0);

			return;
		}

		if (playerPrefab != null)
		{
			StartCoroutine(DelayInitGame());
		}

	}

	void Update()
	{

	}



	#region Photon Callbacks


	public override void OnPlayerEnteredRoom(Player other)
	{

	}

	public override void OnPlayerLeftRoom(Player other)
	{
		
	}

	public override void OnLeftRoom()
	{
		//SceneManager.LoadScene("PunBasics-Launcher");
	}

	#endregion

	#region Public Methods

	public bool LeaveRoom()
	{
		return PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	public void P1GetPoint()
    {
		pv.RPC("RpcP1GetPoint", RpcTarget.All);
	}

	public void P2GetPoint()
    {
		pv.RPC("RpcP2GetPoint", RpcTarget.All);
	}

	public void EndServe()
    {
		pv.RPC("RpcEndServe", RpcTarget.All);
    }

	#endregion

	#region Private Methods

	IEnumerator DelayInitGame()
	{
		print("waiting init");
		yield return new WaitForSeconds(1.0f);
		InitGame();
		print("inited");
	}

	void InitGame()
	{
		if(PhotonNetwork.IsMasterClient)
        {
			myPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-3f, 1.5f, 0f), Quaternion.identity);
			ballObject = PhotonNetwork.InstantiateRoomObject(this.ballPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
			pv.RPC("RpcInitBallObject", RpcTarget.All, ballObject.name);
		}
		else
        {
			myPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(3f, 1.5f, 0f), Quaternion.identity);
			myPlayer.transform.localEulerAngles = new Vector3(0, 180, 0);
		}



	}



	#endregion

	#region PunRPC

	[PunRPC]
	void RpcInitBallObject(string objectName, PhotonMessageInfo info)
    {
		ballObject = GameObject.Find(objectName);
		BallManager bm = ballObject.GetComponent<BallManager>();
		bm.p1StatesPanel = p1StatesPanel;
		bm.p2StatesPanel = p2StatesPanel;
		bm.centerBorder = centerBorder;
	}

	[PunRPC]
	void RpcP1GetPoint(PhotonMessageInfo info)
	{
		GameManager.instance.p1GetPoint();
	}

	[PunRPC]
	void RpcP2GetPoint(PhotonMessageInfo info)
    {
		GameManager.instance.p2GetPoint();
	}

	[PunRPC]
	void RpcEndServe(PhotonMessageInfo info)
	{
		GameManager.instance.EndServe();
	}

	#endregion
}
