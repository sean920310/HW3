using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	[Header("Start Setup")]
	[SerializeField] GameStartManager gameStart;
	[SerializeField] Animator gameStartAnim;
	[SerializeField] Animator hudAnim;
	[SerializeField] GameObject p1ReadyImg;
	[SerializeField] GameObject p2ReadyImg;
	[SerializeField] Button readyBtn;
	[SerializeField] Button nextBtn;
	[SerializeField] Button backBtn;
	[SerializeField] Button startBtn;


	private GameObject myPlayer;
	private GameObject ballObject;
	private PhotonView pv;

	private bool p1Ready = false;
	private bool p2Ready = false;


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
			InitStartSetting();
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

	public void OnP1NameChange(string value)
    {
		if(PhotonNetwork.IsMasterClient)
			pv.RPC("RpcP1NameChange", RpcTarget.Others, value);
    }

	public void OnP2NameChange(string value)
	{
		if(!PhotonNetwork.IsMasterClient)
			pv.RPC("RpcP2NameChange", RpcTarget.Others, value);
	}

	public void OnP1HatChange()
    {
		pv.RPC("RpcP1HatChange", RpcTarget.All, CharacterSlot.player1currentHatIdx);
	}

	public void OnP2HatChange()
	{
		pv.RPC("RpcP2HatChange", RpcTarget.All, CharacterSlot.player2currentHatIdx);
	}

	public void OnScoreChange(int value)
	{
		if(pv)
			pv.RPC("RpcScoreChange", RpcTarget.Others, value);
	}

	public void OnReadyClick()
    {
		if(PhotonNetwork.IsMasterClient)
        {
			pv.RPC("RpcP1Ready", RpcTarget.All, true);
		}
		else
        {
			pv.RPC("RpcP2Ready", RpcTarget.All, true);
		}
		readyBtn.interactable = false;
	}

	public void OnNextClick()
    {
		pv.RPC("RpcNextBtn", RpcTarget.All);
	}

	public void OnBackClick()
	{
		pv.RPC("RpcBackBtn", RpcTarget.All);
	}

	public void OnStartClick()
	{
		pv.RPC("RpcStartBtn", RpcTarget.All);
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

	void InitStartSetting()
    {
		p1ReadyImg.SetActive(false);
		p2ReadyImg.SetActive(false);
		readyBtn.gameObject.SetActive(true);
		nextBtn.gameObject.SetActive(false);
		backBtn.interactable = PhotonNetwork.IsMasterClient;
		startBtn.interactable = PhotonNetwork.IsMasterClient;

		if (PhotonNetwork.IsMasterClient)
        {
			gameStart.Player1NameInput.interactable = true;
			gameStart.scoreToWin.interactable = true; 
			gameStart.characterSlot.p1SlotUI.SetActive(true);
			
			gameStart.Player2NameInput.interactable = false;
			gameStart.characterSlot.p2SlotUI.SetActive(false);

			gameStart.Player1NameInput.text = PhotonNetwork.LocalPlayer.NickName;
			OnP1HatChange();
		}
		else
        {
			gameStart.Player1NameInput.interactable = false;
			gameStart.scoreToWin.interactable = false;
			gameStart.characterSlot.p1SlotUI.SetActive(false);

			gameStart.Player2NameInput.interactable = true;
			gameStart.characterSlot.p2SlotUI.SetActive(true);

			gameStart.Player2NameInput.text = PhotonNetwork.LocalPlayer.NickName;
			OnP2HatChange();
		}
	}

	void CheckBothReady()
    {
		if (p1Ready && p2Ready)
        {
			readyBtn.gameObject.SetActive(false);
			nextBtn.gameObject.SetActive(true);

			nextBtn.interactable = PhotonNetwork.IsMasterClient;
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

	[PunRPC]
	void RpcP1NameChange(string value, PhotonMessageInfo info)
	{
		gameStart.Player1NameInput.text = value;
	}

	[PunRPC]
	void RpcP2NameChange(string value, PhotonMessageInfo info)
	{
		gameStart.Player2NameInput.text = value;
	}

	[PunRPC]
	void RpcP1HatChange(int value, PhotonMessageInfo info)
	{
		CharacterSlot.player1currentHatIdx = value;
		gameStart.characterSlot.UpdateUI();
	}

	[PunRPC]
	void RpcP2HatChange(int value, PhotonMessageInfo info)
	{
		CharacterSlot.player2currentHatIdx = value;
		gameStart.characterSlot.UpdateUI();
	}

	[PunRPC]
	void RpcP1Ready(bool value, PhotonMessageInfo info)
	{
		p1Ready = value;
		p1ReadyImg.SetActive(value);
		CheckBothReady();
	}
	
	[PunRPC]
	void RpcP2Ready(bool value, PhotonMessageInfo info)
	{
		p2Ready = value;
		p2ReadyImg.SetActive(value);
		CheckBothReady();
	}

	[PunRPC]
	void RpcNextBtn(PhotonMessageInfo info)
    {
		gameStartAnim.SetTrigger("Page1Next");
	}

	[PunRPC]
	void RpcBackBtn(PhotonMessageInfo info)
	{
		gameStartAnim.SetTrigger("Page2Back");
	}

	[PunRPC]
	void RpcScoreChange(int value, PhotonMessageInfo info)
	{
		gameStart.scoreToWin.value = value;
	}

	[PunRPC]
	void RpcStartBtn(PhotonMessageInfo info)
	{
		gameStart.SaveSettings();
		gameStart.characterSlot.SaveSettings();
		GameManager.instance.MultiplayerStart();
		hudAnim.SetTrigger("GameStart");
	}

	#endregion
}
