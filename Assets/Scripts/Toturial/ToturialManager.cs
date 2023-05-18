using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ToturialManager : MonoBehaviour
{

    [Serializable]
    public enum ToturialStates
    {
        Beginning,
        ShortServe,
        LongServe,
        Move,
        Underhand,
        Overhand,
        Jump,
        SwitchToAdvenced,
        Smash,
        AdvencedShortServe,
        FrontUnderhand,
        BackUnderhand,
        End
    }

    static public ToturialManager Instance;

    [SerializeField] private RectTransform endPanel;

    public MessageBoxSO[] messageBoxList;
    [SerializeField] private MessageBoxItem messageBoxPrefeb;
    [SerializeField] private RectTransform messageBoxSpawnPoint;

    [SerializeField] int messageBoxIdx = 0;


    [Header("ObjectForTutorial")]
    [SerializeField] GameManager gm;
    [SerializeField] PlayerMovement player;
    [SerializeField] PlayerMovement enemy;
    [SerializeField] BallManager ball;

    [SerializeField] float EnemyMovementDelay = 1;


    [Header("Animation")]
    [SerializeField] float typingSpeed;
    private bool isPlayingAnimation = false;
    private bool animationEndNow = false;

    public ToturialStates toturialStates;

    [SerializeField] public UnityEngine.UI.Toggle TutorialToggle;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if (messageBoxList.Length > messageBoxIdx)
        {
            SpawnMessageBox(messageBoxList[messageBoxIdx].messageBoxData);
            toturialStates = messageBoxList[messageBoxIdx].messageBoxData.toturialStates;
        }

        GameManager.instance.toturialSetup();
        GameManager.instance.SetEndless();
    }

    // Update is called once per frame
    void Update()
    {

        if (messageBoxList.Length <= messageBoxIdx)
        {
            endPanel.gameObject.SetActive(true);

            if(GameSettingsManager.instance != null)
            {
                TutorialToggle.isOn = GameSettingsManager.instance.settingsData.tutorial = false;
            }
            return;
        }

        if (!isPlayingAnimation)
        {
            AllStateSwitchState();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlayingAnimation)
            {
                animationEndNow = true;
            }
            else if (!(messageBoxList.Length - 1 > messageBoxIdx))
            {
                // Final
                RemoveMessageBoxItemUI();
            }
        }

    }

    private void AllStateSwitchState()
    {
        if (!(messageBoxList.Length - 1 > messageBoxIdx) && isPlayingAnimation)
        {
            return;
        }
        if (hitPlayerGround)
        {
            PlayerActionIncorrect();
            return;
        }
        switch (toturialStates)
        {
            case ToturialStates.Beginning:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PlayerActionCorrect();
                    resetAllFlag();
                    AllStateSetUp();
                }
                break;
            case ToturialStates.ShortServe:

                if (swinUpInputFlag)
                {
                    PlayerActionIncorrect();
                    return;
                }
                if (swinDownInputFlag && hitEnemyGround)
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.LongServe:
                if (swinDownInputFlag)
                {
                    PlayerActionIncorrect();
                    return;
                }
                if (swinUpInputFlag && hitEnemyGround)
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.Move:
                if (moveLeft && moveRight)
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.Underhand:
                if (hitBall && hitEnemyGround)
                {
                    if (!swinDownInputFlag)
                    {
                        PlayerActionIncorrect();
                        return;
                    }
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.Overhand:
                if (hitBall && hitEnemyGround)
                {
                    if (!swinUpInputFlag)
                    {
                        PlayerActionIncorrect();
                        return;
                    }
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.Jump:
                if (jumpInputFlag)
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.SwitchToAdvenced:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.Smash:
                if (hitEnemyGround)
                {
                    if (!smash)
                    {
                        PlayerActionIncorrect();
                        return;
                    }

                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.AdvencedShortServe:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.FrontUnderhand:
                if (hitEnemyGround)
                {
                    if (!underhandFront)
                    {
                        PlayerActionIncorrect();
                        return;
                    }

                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.BackUnderhand:
                if (hitEnemyGround)
                {
                    if (!underhandBack)
                    {
                        PlayerActionIncorrect();
                        return;
                    }

                    PlayerActionCorrect();
                }
                break;
            case ToturialStates.End:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PlayerActionCorrect();
                }
                break;
            default:
                break;
        }
    }

    private void PlayerActionIncorrect()
    {
        CurrentMsgAgain();

        player.setAnimationToIdle();
        enemy.setAnimationToIdle();

        resetAllFlag();
        AllStateSetUp();
    }

    private void AllStateSetUp()
    {
        player.setAnimationToIdle();
        enemy.setAnimationToIdle();

        switch (toturialStates)
        {
            case ToturialStates.Beginning:
                break;
            case ToturialStates.ShortServe:

                ball.gameObject.SetActive(true);
                player.SetPlayerServe(true);
                enemy.SetPlayerServe(false);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                break;
            case ToturialStates.LongServe:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(true);
                enemy.SetPlayerServe(false);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                break;
            case ToturialStates.Move:
                ball.gameObject.SetActive(false);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                break;
            case ToturialStates.Underhand:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                StartCoroutine(enemyOverhand());
                break;
            case ToturialStates.Overhand:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                StartCoroutine(enemyOverhand());
                break;
            case ToturialStates.Jump:
                ball.gameObject.SetActive(false);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                break;
            case ToturialStates.SwitchToAdvenced:
                break;
            case ToturialStates.Smash:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                StartCoroutine(enemyOverhand());
                break;
            case ToturialStates.AdvencedShortServe:
                break;
            case ToturialStates.FrontUnderhand:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                StartCoroutine(enemyUnderhand());
                break;
            case ToturialStates.BackUnderhand:
                ball.gameObject.SetActive(true);
                player.SetPlayerServe(false);
                enemy.SetPlayerServe(true);
                BallManager.Instance.SwitchState(BallManager.BallStates.Serving);
                StartCoroutine(enemyOverhand());
                break;
            case ToturialStates.End:
                break;
            default:
                break;
        }
    }

    private void PlayerActionCorrect()
    {
        RemoveMessageBoxItemUI();


        messageBoxIdx++;
        if (messageBoxList.Length <= messageBoxIdx)
            return;
        SpawnMessageBox(messageBoxList[messageBoxIdx].messageBoxData);
        toturialStates = messageBoxList[messageBoxIdx].messageBoxData.toturialStates;

        player.setAnimationToIdle();
        enemy.setAnimationToIdle();

        resetAllFlag();
        AllStateSetUp();
    }

    private void CurrentMsgAgain()
    {
        RemoveMessageBoxItemUI();
        SpawnMessageBox(messageBoxList[messageBoxIdx].messageBoxData);
        toturialStates = messageBoxList[messageBoxIdx].messageBoxData.toturialStates;
    }

    void SpawnMessageBox(MessageBoxData mbd)
    {
        MessageBoxItem tmpMB = Instantiate(messageBoxPrefeb);
        tmpMB.transform.SetParent(messageBoxSpawnPoint, false);
        tmpMB.SetText("", "");
        StartCoroutine( messageBoxAnimation(tmpMB, mbd.HintText, mbd.ActionHintText));
    }

    void RemoveMessageBoxItemUI()
    {
        if(messageBoxSpawnPoint.childCount != 0)
            Destroy(messageBoxSpawnPoint.GetChild(0).gameObject);
    }

    void resetAllFlag()
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

    IEnumerator messageBoxAnimation(MessageBoxItem msgBox, string hintText, string actionHintText)
    {
        string tmpHintText = hintText;
        isPlayingAnimation = true;
        animationEndNow = false;
        while (tmpHintText.Length > 0 && msgBox != null)
        {
            msgBox.AddHintText(tmpHintText[0]);
            yield return new WaitForSeconds(typingSpeed);
            tmpHintText = tmpHintText.Remove(0, 1);

            if (animationEndNow)
            {
                tmpHintText = "";
            }
        }
        if(msgBox != null)
            msgBox.SetText(hintText, actionHintText);
        isPlayingAnimation = false;
    }

    IEnumerator enemyOverhand()
    {
        while(isPlayingAnimation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(EnemyMovementDelay);
        enemy.swinUpInputFlag= true;
    }
    IEnumerator enemyUnderhand()
    {
        while (isPlayingAnimation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(EnemyMovementDelay);
        enemy.swinDownInputFlag = true;
    }
}
