using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;


public class PhotonManager : MonoBehaviourPunCallbacks
{
	static public PhotonManager Instance;

	[Header("Player")]
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject player1Bot;
	[SerializeField] GameObject player2Bot;

	[Header("Ball")]
	[SerializeField] GameObject ballPrefab;
	
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

	[Header("Loading Scene")]
	[SerializeField] LoadingScene loadingScene;


	public GameManager.Players players { get; private set; }

	private bool inited = false;	//gameobject inited
	private bool rejoin = false;	//player rejoin

	private GameObject myPlayer;
	private GameObject ballObject;
	private PhotonView pv;

	private Player player1;
	private Player player2;

	private bool p1Ready = false;
	private bool p2Ready = false;

	private bool p1Leave = false;
	private bool p2Leave = false;

	private Dictionary<Player, HashTable> playersInfo;


	void Start()
	{
		Instance = this;

		pv = GetComponent<PhotonView>();
		playersInfo = new Dictionary<Player, HashTable>();

		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene(0);

			return;
		}

		if (playerPrefab != null)
		{
			players = (PhotonNetwork.IsMasterClient) ? GameManager.Players.Player1 : GameManager.Players.Player2;
			StartCoroutine(DelayInitGame());
			InitStartSetting();
		}


	}

	void Update()
	{

	}



    #region Photon Callbacks

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
		//if (targetPlayer == PhotonNetwork.LocalPlayer) return;
		if (playersInfo.ContainsKey(targetPlayer))
			playersInfo[targetPlayer] = changedProps;
		else
			playersInfo.Add(targetPlayer, changedProps);

		if (!inited) return;

		PlayerInformationManager player;
		if (player1 == targetPlayer)
        {
			player = GameManager.instance.Player1.GetComponent<PlayerInformationManager>();
		}
		else
        {
			player = GameManager.instance.Player2.GetComponent<PlayerInformationManager>();
		}

		player.Info.name = (string)changedProps["name"];
		player.Info.score = (int)changedProps["score"];
		player.Info.smashCount = (int)changedProps["smashCount"];
		player.Info.defenceCount = (int)changedProps["defenceCount"];
		player.Info.overhandCount = (int)changedProps["overhandCount"];
		player.Info.underhandCount = (int)changedProps["underhandCount"];
	}

    public override void OnPlayerEnteredRoom(Player other)
	{
		if(OneSideLeave() && players != GameManager.Players.None)
        {
			pv.RPC("RpcInitBallObject", other, ballObject.name);

			int playerToJoin = (p1Leave) ? 1 : 2;
			int scoreToWin = gameStart.scoreToWin.value;
			pv.RPC("RpcRejoin", other, (int)GameManager.instance.gameState, playerToJoin, scoreToWin);

			if (p1Leave)
            {
				PlayerInfoUpdate(pv.Owner, GameManager.instance.Player2.GetComponent<PlayerInformationManager>());
				PlayerInfoUpdate(other, GameManager.instance.Player1.GetComponent<PlayerInformationManager>());
            }
			else
            {
				PlayerInfoUpdate(pv.Owner, GameManager.instance.Player1.GetComponent<PlayerInformationManager>());
				PlayerInfoUpdate(other, GameManager.instance.Player2.GetComponent<PlayerInformationManager>());
			}

		}
	}

	public override void OnPlayerLeftRoom(Player other)
	{
		if (other == player1)
        {
			print("P1 leave");
			//Player1

			//Master Leave
			ballObject.GetPhotonView().TransferOwnership(player2);
			PhotonNetwork.SetMasterClient(player2);
			PlayerInformationManager.PlayerInfo info = GameManager.instance.Player1.GetComponent<PlayerInformationManager>().Info;
			PhotonNetwork.Destroy(GameManager.instance.Player1);

			player1Bot.SetActive(true);
			GameManager.instance.LoadPlayer1(player1Bot);
			player1Bot.GetComponent<PlayerInformationManager>().Info = info;
			if (BallManager.Instance.IsInState(BallManager.BallStates.Serving) && GameManager.instance.Serving == GameManager.Players.Player1)
			{
				GameManager.instance.SetServePlayer(GameManager.Players.Player1);
			}
			p1Leave = true;
		}
        else if (other == player2)
		{
			print("P2 leave");
			PlayerInformationManager.PlayerInfo info = GameManager.instance.Player2.GetComponent<PlayerInformationManager>().Info;
			PhotonNetwork.Destroy(GameManager.instance.Player2);

			//Player2
			player2Bot.SetActive(true);
			GameManager.instance.LoadPlayer2(player2Bot);
			player2Bot.GetComponent<PlayerInformationManager>().Info = info;
			if (BallManager.Instance.IsInState(BallManager.BallStates.Serving) && GameManager.instance.Serving == GameManager.Players.Player2)
            {
				GameManager.instance.SetServePlayer(GameManager.Players.Player2);
			}
			p2Leave = true;
		}

	}

	public override void OnLeftRoom()
	{
		loadingScene.LoadScene(0);
	}

	#endregion

	#region Public Methods

	public bool IsPlayer1()
    {
		return players == GameManager.Players.Player1;
	}

	//On PlayerObject Instantiate by Photon
	public void PlayerInstantiate(GameObject playerObject, bool isMine)
	{
		if(p1Leave)
        {
			GameManager.instance.Player1.SetActive(false);
			GameManager.instance.LoadPlayer1(playerObject);
			player1 = playerObject.GetPhotonView().Owner;
			p1Leave = false;
			return;
		}
		if(p2Leave)
        {
			GameManager.instance.Player2.SetActive(false);
			GameManager.instance.LoadPlayer2(playerObject);
			player2 = playerObject.GetPhotonView().Owner;
			p2Leave = false;
			return;
		}

		if ((isMine && IsPlayer1()) || (!isMine && !IsPlayer1()))
		{
			GameManager.instance.Player1 = playerObject;
			player1 = playerObject.GetPhotonView().Owner;
		}
		else
		{
			GameManager.instance.Player2 = playerObject;
			player2 = playerObject.GetPhotonView().Owner;
		}
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

	public void GameOver()
	{
		pv.RPC("RpcGameOver", RpcTarget.All);
	}

	public void PlayerInfoUpdate(Player player, PlayerInformationManager info)
    {
		HashTable playerInfoHashTable = new HashTable();
		playerInfoHashTable.Add("name", info.Info.name);
		playerInfoHashTable.Add("score", info.Info.score);
		playerInfoHashTable.Add("smashCount", info.Info.smashCount);
		playerInfoHashTable.Add("defenceCount", info.Info.defenceCount);
		playerInfoHashTable.Add("overhandCount", info.Info.overhandCount);
		playerInfoHashTable.Add("underhandCount", info.Info.underhandCount);
		player.SetCustomProperties(playerInfoHashTable);
    }

	public bool OneSideLeave()
    {
		return p1Leave || p2Leave;
    }


	#endregion

	#region Private Methods

	IEnumerator DelayInitGame()
	{
		print("waiting init");
		yield return new WaitForSeconds(1.0f);
		InitGame();
		print("inited");
		inited = true;
	}

	void InitGame()
	{
		if(players == GameManager.Players.Player1)
        {
			myPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-3f, 1.5f, 0f), Quaternion.identity);
			if (!rejoin)
            {
				ballObject = PhotonNetwork.Instantiate(this.ballPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
				pv.RPC("RpcInitBallObject", RpcTarget.All, ballObject.name);
            }
		}
		else if (players == GameManager.Players.Player2)
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

	IEnumerator WaitInitAndRejoin(int scoreToWin)
    {
		while (!inited)
			yield return null;

		gameStart.scoreToWin.value = scoreToWin;

		OnPlayerPropertiesUpdate(player1, playersInfo[player1]);
		OnPlayerPropertiesUpdate(player2, playersInfo[player2]);

		if(GameManager.instance.gameState == GameManager.GameStates.InGame)
        {
			GameManager.instance.MultiplayerStart();
			hudAnim.SetTrigger("GameStart");
        }
	}

	#endregion

	#region Button Callback

	public void OnP1NameChange(string value)
	{
		if (pv && PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.NickName = value;
			pv.RPC("RpcP1NameChange", RpcTarget.Others, value);
        }
	}

	public void OnP2NameChange(string value)
	{
		if (pv && !PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.NickName = value;
			pv.RPC("RpcP2NameChange", RpcTarget.Others, value);
        }
	}

	public void OnP1HatChange()
	{
		if (pv)
			pv.RPC("RpcP1HatChange", RpcTarget.All, CharacterSlot.player1currentHatIdx);
	}

	public void OnP2HatChange()
	{
		if (pv)
			pv.RPC("RpcP2HatChange", RpcTarget.All, CharacterSlot.player2currentHatIdx);
	}

	public void OnScoreChange(int value)
	{
		if (pv)
			pv.RPC("RpcScoreChange", RpcTarget.Others, value);
	}

	public void OnReadyClick()
	{
		if (PhotonNetwork.IsMasterClient)
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

	public void OnRematchClick()
	{
		pv.RPC("RpcRematch", RpcTarget.All);
	}

	public void OnQuitClick()
	{
		PhotonNetwork.LeaveRoom();
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

	[PunRPC]
	void RpcGameOver(PhotonMessageInfo info)
    {
		GameManager.instance.GameOver();
    }

	[PunRPC]
	void RpcRematch(PhotonMessageInfo info)
	{
		PhotonNetwork.DestroyAll();
		PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
	}

	[PunRPC]
	void RpcRejoin(int gameState,int playerToJoin, int scoreToWin, PhotonMessageInfo info)
    {
		print("rejoin");
		rejoin = true;
		GameManager.instance.gameState = (GameManager.GameStates)gameState;
		if (playerToJoin == 1)
		{
			players = GameManager.Players.Player1;
			if(GameManager.instance.Player1)
            {
				//如果對方object已經被放錯位置
				GameManager.instance.Player2 = GameManager.instance.Player1;
				player2 = info.Sender;
            }				
		}
		else
        {
			players = GameManager.Players.Player2;
			if (GameManager.instance.Player2)
			{
				//如果對方object已經被放錯位置
				GameManager.instance.Player1 = GameManager.instance.Player2;
				player1 = info.Sender;
			}
		}

		StartCoroutine(WaitInitAndRejoin(scoreToWin));
	}

	#endregion
}
