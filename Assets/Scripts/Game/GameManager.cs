using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        GamePreparing,
        InGame,
        GamePause,
        GameOver,
    }
    public enum Players
    {
        Player1,
        Player2,
        None,
    }

    public static GameManager instance { get; private set; }

    [Header("Game Information")]
    [SerializeField] GameStartManager gameStarManager;
    [ReadOnly] [SerializeField] GameStates gameState;
    [SerializeField] int winScore;
    [SerializeField] bool neverFinish; // Endless if true

    [Header("GameObject")]
    [SerializeField] public GameObject Player1;
    [SerializeField] public GameObject Player2;
    //[SerializeField] BallManager Ball;

    [SerializeField] GameObject ServeBorderL;
    [SerializeField] GameObject ServeBorderR;

    [Header("UI")]
    [SerializeField] RectTransform GameoverPanel;
    [SerializeField] HUDPanel HUD;
    [SerializeField] RectTransform PausePanel;
    [SerializeField] RectTransform GameStartPanel;

    [Header("Audio")]
    [SerializeField] AudioSource PlayerOneWinSound;
    [SerializeField] AudioSource PlayerTwoWinSound;
    [SerializeField] AudioSource GameoverCheeringSound;

    [Header("Light")]
    [SerializeField] Light directionalLight;
    [SerializeField] Light spotLight;

    public bool isMultiplayer = false;

    public Players Winner { get; private set; } = Players.None;

    private PlayerMovement Player1Movement;
    private PlayerMovement Player2Movement;
    private PlayerInformationManager Player1Info;
    private PlayerInformationManager Player2Info;
    private Transform Player1HatPoint;
    private Transform Player2HatPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one GameManager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        //get component
        if(Player1)
        {
            Player1Movement = Player1.GetComponent<PlayerMovement>();
            Player1Info = Player1.GetComponent<PlayerInformationManager>();
            Player1HatPoint = Player1.transform.Find("Body/Neck/Head/HatPoint");
        }
        if (Player2)
        {
            Player2Movement = Player2.GetComponent<PlayerMovement>();
            Player2Info = Player2.GetComponent<PlayerInformationManager>();
            Player2HatPoint = Player2.transform.Find("Body/Neck/Head/HatPoint");
        }

        //Time.timeScale = 0.0f;
        gameState = GameStates.GamePreparing;
        neverFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameState != GameStates.GamePreparing)
            {
                if (gameState == GameStates.GamePause) Resume();
                else Pause();
            }
        }

        if(Player1Movement && Player2Movement)
        {
            if (!Player1Movement.PrepareServe && !Player2Movement.PrepareServe)
            {
                ServeBorderActive(false);
            }
        }
    }

    private bool CheckIsGameover()
    {
        return !neverFinish && gameState != GameStates.GameOver && (Player1Info.Info.score >= winScore || Player2Info.Info.score >= winScore);
    }

    public void p1GetPoint()
    {
        Player1Info.Info.score++;

        // UI Update
        HUD.P1IsAboutToWin = (Player1Info.Info.score == winScore - 1); 
        HUD.ScorePanelUpdate(Player1Info.Info.score, Player2Info.Info.score);
        HUD.SetServeHint(true, false);

        // Set Player State 
        SetServePlayer(Players.Player1);
        playerStatesReset();
        StartCoroutine(PlayerMovementDisableForAWhile(0.5f));

        // Set ball Serve State to true
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);

        ServeBorderActive(true);

        // Check if the game over condition has been satisfied.
        if (CheckIsGameover())
        {
            GameOver();
        }
    }

    public void p2GetPoint()
    {
        Player2Info.Info.score++;

        // UI Update
        HUD.P2IsAboutToWin = (Player2Info.Info.score == winScore - 1);
        HUD.ScorePanelUpdate(Player1Info.Info.score, Player2Info.Info.score);
        HUD.SetServeHint(false, true);

        // Set Player State 
        SetServePlayer(Players.Player2);
        playerStatesReset();
        StartCoroutine(PlayerMovementDisableForAWhile(0.5f));

        // Set ball Serve State to true
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);

        ServeBorderActive(true);

        // Check if the game over condition has been satisfied.
        if (CheckIsGameover())
        {
            GameOver();
        }
    }

    private void SetServePlayer(Players ServePlayer)
    {
        if(ServePlayer == Players.Player1)
        {
            Player1Movement.SetPlayerServe(true);
        }
        else
        {
            Player2Movement.SetPlayerServe(true);
        }
    }

    // Serve Border will active whenever player is prepare to serve.
    private void ServeBorderActive(bool active)
    {
        ServeBorderL.SetActive(active);
        ServeBorderR.SetActive(active);
    }

    public void playerStatesReset()
    {
        Player1Movement.ResetAllAnimatorTriggers();
        Player2Movement.ResetAllAnimatorTriggers();

        Player1Movement.animator.Play("Idle", -1, 0.0f);
        Player2Movement.animator.Play("Idle", -1, 0.0f);

        Player1Movement.transform.localPosition = new Vector3(-3, 1.06f, 0);
        Player2Movement.transform.localPosition = new Vector3(3, 1.06f, 0);

        Player1Movement.rb.velocity = new Vector3(0, 0f, 0);
        Player2Movement.rb.velocity = new Vector3(0, 0f, 0);

        Player1Movement.ResetInputFlag();
        Player2Movement.ResetInputFlag();
    }

    public void GameOver()
    {
        gameState = GameStates.GameOver;


        // Set Animator UpdateMode to UnscaledTime inorder to play dance animation.
        Player1Movement.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Player2Movement.animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (Player1Info.Info.score > Player2Info.Info.score)
        {
            PlayerOneWinSound.Play();
            Winner = Players.Player1;
            Player1Movement.animator.SetTrigger("Dancing1");
            Player2Movement.animator.SetTrigger("Lose");
        }
        else if (Player1Info.Info.score < Player2Info.Info.score)
        {
            PlayerTwoWinSound.Play();
            Winner = Players.Player2;
            Player1Movement.animator.SetTrigger("Lose");
            Player2Movement.animator.SetTrigger("Dancing1");
        }
        else
        {
            Winner = Players.None;
            Player1Movement.animator.SetTrigger("Dancing1");
            Player2Movement.animator.SetTrigger("Dancing1");
        }

        // Set Light
        directionalLight.intensity = 0.1f;
        spotLight.gameObject.SetActive(true);
        switch (Winner)
        {
            case Players.Player1:
                spotLight.transform.position = new Vector3( Player1Movement.transform.position.x, spotLight.transform.position.y, spotLight.transform.position.z);
                break;
            case Players.Player2:
                spotLight.transform.position = new Vector3(Player2Movement.transform.position.x, spotLight.transform.position.y, spotLight.transform.position.z);
                break;
            case Players.None:
                break;
            default:
                break;
        }

        GameoverCheeringSound.Play();
        //Time.timeScale = 0.0f;
        HUD.GetComponent<Animator>().SetTrigger("GameEnd");
        HUD.SetServeHint(false, false);
        GameoverPanel.gameObject.SetActive(true);

        Player1Movement.enabled = false;
        Player2Movement.enabled = false;
    }

    public void Pause()
    {
        gameState = GameStates.GamePause;
        //Time.timeScale = 0.0f;
        PausePanel.gameObject.SetActive(true);
    }

    private void Resume()
    {
        gameState = GameStates.InGame;
        Time.timeScale = 1.0f;
        PausePanel.gameObject.SetActive(false);
    }

    IEnumerator PlayerMovementDisableForAWhile(float delay)
    {
        Player1Movement.swinDisable();
        Player2Movement.swinDisable();
        Player1Movement.ResetInputFlag();
        Player2Movement.ResetInputFlag();

        yield return new WaitForSeconds(delay);
        Player1Movement.swinEnable();
        Player2Movement.swinEnable();
        Player1Movement.ResetInputFlag();
        Player2Movement.ResetInputFlag();
    }

    #region Button_Event
    public void OnQuitClick()
    {
        SceneManager.LoadScene(0);
    }
    public void OnRematchClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnStartClick()
    {
        GameStart();
    }

    public void MultiplayerStart()
    {
        GameStart(true);
    }

    public void EndServe()
    {
        HUD.SetServeHint(false, false);
        Player1Movement.SetPlayerServe(false);
        Player2Movement.SetPlayerServe(false);
    }

    private void GameStart(bool isMultiplayer = false)
    {
        if (Player1)
        {
            Player1Movement = Player1.GetComponent<PlayerMovement>();
            Player1Info = Player1.GetComponent<PlayerInformationManager>();
            Player1HatPoint = Player1.transform.Find("Body/Neck/Head/HatPoint");
        }
        if (Player2)
        {
            Player2Movement = Player2.GetComponent<PlayerMovement>();
            Player2Info = Player2.GetComponent<PlayerInformationManager>();
            Player2HatPoint = Player2.transform.Find("Body/Neck/Head/HatPoint");
        }

        // Get Bot Enable.
        if (!isMultiplayer && gameStarManager.P1BotToggle.isOn)    
            Player1Movement.GetComponent<BotManager>().enabled = true;
        if (!isMultiplayer && gameStarManager.P2BotToggle.isOn)
            Player2Movement.GetComponent<BotManager>().enabled = true;

        // Get Info From Game Start Setting.
        Player1Info.Info.name = gameStarManager.Player1NameInput.text;
        Player2Info.Info.name = gameStarManager.Player2NameInput.text;

        // Set Hat.
        if (CharacterSlot.HatList[CharacterSlot.player1currentHatIdx].hatData.HatPrefab != null)
        {
            GameObject tmpHatPrefab = GameObject.Instantiate(CharacterSlot.HatList[CharacterSlot.player1currentHatIdx].hatData.HatPrefab);
            tmpHatPrefab.transform.SetParent(Player1HatPoint, false);
        }
        if (CharacterSlot.HatList[CharacterSlot.player2currentHatIdx].hatData.HatPrefab != null)
        {
            GameObject tmpHatPrefab = GameObject.Instantiate(CharacterSlot.HatList[CharacterSlot.player2currentHatIdx].hatData.HatPrefab);
            tmpHatPrefab.transform.SetParent(Player2HatPoint, false);
        }

        // Ser Winning Score.
        if (gameStarManager.scoreToWin.options.ToArray()[gameStarManager.scoreToWin.value].text != "Endless")
            int.TryParse(gameStarManager.scoreToWin.options.ToArray()[gameStarManager.scoreToWin.value].text, out winScore);
        else
            neverFinish = true;

        GameStartPanel.gameObject.SetActive(false);

        Player1Movement.gameObject.SetActive(true);
        Player2Movement.gameObject.SetActive(true);
        BallManager.Instance.gameObject.SetActive(true);

        // Set Player State 
        SetServePlayer(Players.Player1);
        HUD.SetServeHint(true, false);

        Player1Movement.transform.localPosition = new Vector3(-3, 1.06f, 0);
        Player2Movement.transform.localPosition = new Vector3(3, 1.06f, 0);

        StartCoroutine(PlayerMovementDisableForAWhile(0.3f));

        // Set ball Serve State to true
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);

        ServeBorderActive(true);

        // HUD Update
        HUD.ScorePanelUpdate(Player1Info.Info.score, Player2Info.Info.score);
        HUD.PlayerNameUpdate(Player1Info.Info.name, Player2Info.Info.name);
        HUD.PlayerHatUpdate(CharacterSlot.HatList[CharacterSlot.player1currentHatIdx].hatData.HatSprite,
                            CharacterSlot.HatList[CharacterSlot.player2currentHatIdx].hatData.HatSprite);

        // Check if the game over condition has been satisfied.
        if (CheckIsGameover())
        {
            GameOver();
        }

        Time.timeScale = 1.0f;

        gameState = GameStates.InGame;
    }

    // Pause Panel
    public void OnResumeClick()
    {
        Resume();
    }

    public void OnEndGameClick()
    {
        PausePanel.gameObject.SetActive(false);
        GameOver();
    }
    #endregion
}
