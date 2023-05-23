using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TutorialManager : MonoBehaviour
{
    #region Tutorial States Machine
    static public TutorialManager Instance;
    private TutorialStatesFactory StatesFactory;
    public TutorialStateBase currentState;
    #endregion

    #region Message Box
    public MessageBoxSO[] messageBoxList;
    [SerializeField] private MessageBoxItem messageBoxPrefeb;
    [SerializeField] private RectTransform messageBoxSpawnPoint;
    [SerializeField] int messageBoxIdx = 0;
    #endregion

    [Header("ObjectForTutorial")]
    [SerializeField] GameManager gm;
    [SerializeField] PlayerMovement player;
    [SerializeField] PlayerMovement enemy;
    [SerializeField] BallManager ball;

    [SerializeField] float EnemyMovementDelay = 1;

    private bool enemyWaitingToAttack = false;

    [Header("Animation")]
    [SerializeField] private float typingSpeed;
    private bool isPlayingAnimation = false;
    private bool animationEndNow = false;

    [SerializeField] private RectTransform endPanel;

    [SerializeField] private UnityEngine.UI.Toggle TutorialToggle;

    #region Toturial flag

    public bool moveRight = false;
    public bool moveLeft = true;
    public bool jumpInputFlag = false;
    public bool swinUpInputFlag = false;
    public bool swinDownInputFlag = false;
    public bool smash = false;
    public bool hitEnemyGround = false;
    public bool hitPlayerGround = false;
    public bool hitBall = false;
    public bool underhandFront = false;
    public bool underhandBack = false;

    #endregion

    void Start()
    {
        // State Machine Setup.
        Instance = this;
        StatesFactory = new TutorialStatesFactory(this);

        // Spawn the first msgbox.
        if (messageBoxIdx < messageBoxList.Length)
        {
            MessageBoxData messageBoxData = messageBoxList[messageBoxIdx].messageBoxData;
            SpawnMessageBox(messageBoxData);
            currentState = StatesFactory.CreateObject(messageBoxData.toturialStates);

            currentState.EnterState();
        }

        GameManager.instance.toturialSetup();
        GameManager.instance.SetEndless();
    }

    void Update()
    {
        // Tutorial end.
        if (messageBoxList.Length <= messageBoxIdx)
        {
            RemoveMessageBoxItemUI();

            endPanel.gameObject.SetActive(true);

            // Set Tutorial setting to true.
            if (GameSettingsManager.instance != null)
            {
                TutorialToggle.isOn = GameSettingsManager.instance.settingsData.tutorial = false;
            }
            return;
        }

        // The tutorial state update will wait for the animation.
        if (!isPlayingAnimation)
        {
            currentState.SwitchState();
        }
        else
        {
            // Press Enter to skip animation. 
            if (Input.GetKeyDown(KeyCode.Return))
            {
                animationEndNow = true;
            }
        }
    }

    // Two types of state switching.
    public void SwitchToNextMsg()
    {
        resetAllFlag();

        // MsgBox update && Fetch Tutorial state from MsgBoxData.
        RemoveMessageBoxItemUI();
        messageBoxIdx++;
        if (messageBoxIdx < messageBoxList.Length)
        {
            MessageBoxData messageBoxData = messageBoxList[messageBoxIdx].messageBoxData;
            SpawnMessageBox(messageBoxData);
            currentState = StatesFactory.CreateObject(messageBoxData.toturialStates);
            StartCoroutine(WaitAnimationEndedToEnterState());
        }

        // Reset Player State
        player.ResetAllAnimatorTriggers();
        player.setAnimationToIdle();

        enemy.ResetAllAnimatorTriggers();
        enemy.setAnimationToIdle();
    }

    public void RepeatCurrentMsg()
    {
        resetAllFlag();

        //RemoveMessageBoxItemUI();
        //SpawnMessageBox(messageBoxList[messageBoxIdx].messageBoxData);

        StartCoroutine(WaitAnimationEndedToEnterState());

        // Reset Player State
        player.ResetAllAnimatorTriggers();
        player.setAnimationToIdle();

        enemy.ResetAllAnimatorTriggers();
        enemy.setAnimationToIdle();
    }

    // For State Setup.
    public void NoActionSetup()
    {
        player.CanSwin = false;
        ball.gameObject.SetActive(false);
        player.SetPlayerServe(false);
        enemy.SetPlayerServe(false);
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
    }
    public void PlayerServeSetup()
    {
        player.CanSwin = true;
        enemy.CanSwin = true;
        ball.gameObject.SetActive(true);
        player.SetPlayerServe(true);
        enemy.SetPlayerServe(false);
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
    }
    public void EnemyOverhandSetup()
    {
        player.CanSwin = true;
        enemy.CanSwin = true;
        ball.gameObject.SetActive(true);
        player.SetPlayerServe(false);
        enemy.SetPlayerServe(true);
        BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
        StartCoroutine(enemyOverhand());
    }
    public void EnemyUnderhandSetup()
    {
        player.CanSwin = true;
        enemy.CanSwin = true;
        ball.gameObject.SetActive(true);
        player.SetPlayerServe(false);
        enemy.SetPlayerServe(true);
        StartCoroutine(enemyUnderhand());
    }

    private void SpawnMessageBox(MessageBoxData mbd)
    {
        MessageBoxItem tmpMB = Instantiate(messageBoxPrefeb);
        tmpMB.transform.SetParent(messageBoxSpawnPoint, false);
        tmpMB.SetText("", "");
        StartCoroutine( messageBoxAnimation(tmpMB, mbd.HintText, mbd.ActionHintText));
    }
    private void RemoveMessageBoxItemUI()
    {
        if(messageBoxSpawnPoint.childCount != 0)
            Destroy(messageBoxSpawnPoint.GetChild(0).gameObject);
    }
    private void resetAllFlag()
    {
        moveRight = false;
        moveLeft = true;
        jumpInputFlag = false;
        swinUpInputFlag = false;
        swinDownInputFlag = false;
        smash = false;
        hitEnemyGround = false;
        hitPlayerGround = false;
        hitBall = false;
        underhandFront = false;
        underhandBack = false;

        animationEndNow = false;
    }

    #region Coroutine

    private IEnumerator WaitAnimationEndedToEnterState()
    {
        while (isPlayingAnimation)
        {
            yield return null;
        }

        yield return null;

        currentState.EnterState();
    }

    // Message Box Animation
    IEnumerator messageBoxAnimation(MessageBoxItem msgBox, string hintText, string actionHintText)
    {
        if (isPlayingAnimation)
        {
            msgBox.SetText(hintText, actionHintText);
            isPlayingAnimation = false;
            StopCoroutine(messageBoxAnimation(msgBox, hintText, actionHintText));
        }

        string tmpHintText = hintText;
        isPlayingAnimation = true;
        animationEndNow = false;
        while (tmpHintText.Length > 0 && msgBox != null)
        {
            isPlayingAnimation = true;
            msgBox.AddHintText(tmpHintText[0]);
            yield return new WaitForSeconds(typingSpeed);
            tmpHintText = tmpHintText.Remove(0, 1);

            if (animationEndNow)
            {
                tmpHintText = "";
            }
        }
        if (msgBox != null)
            msgBox.SetText(hintText, actionHintText);
        isPlayingAnimation = false;
    }

    // Enemy Movement
    IEnumerator enemyOverhand()
    {
        if (enemyWaitingToAttack)
        {
            enemyWaitingToAttack = false;
            StopCoroutine(enemyOverhand());
        }

        enemyWaitingToAttack = true;
        while (isPlayingAnimation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(EnemyMovementDelay);
        enemy.swinUpInputFlag = true;
        enemyWaitingToAttack = false;
    }
    IEnumerator enemyUnderhand()
    {
        if (enemyWaitingToAttack)
        {
            enemyWaitingToAttack = false;
            StopCoroutine(enemyUnderhand());
        }

        enemyWaitingToAttack = true;
        while (isPlayingAnimation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(EnemyMovementDelay);
        enemy.swinDownInputFlag = true; 
        enemyWaitingToAttack = false;
    }
    #endregion
}
