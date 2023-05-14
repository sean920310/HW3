using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private TextMeshProUGUI P1NameHeader;
    [SerializeField] private TextMeshProUGUI P2NameHeader;

    [SerializeField] private TextMeshProUGUI P1ScoreText;
    [SerializeField] private TextMeshProUGUI P2ScoreText;

    [SerializeField] private TextMeshProUGUI P1SmashText;
    [SerializeField] private TextMeshProUGUI P2SmashText;

    [SerializeField] private TextMeshProUGUI P1DefenseText;
    [SerializeField] private TextMeshProUGUI P2DefenseText;

    [SerializeField] private TextMeshProUGUI P1OverhandText;
    [SerializeField] private TextMeshProUGUI P2OverhandText;

    [SerializeField] private TextMeshProUGUI P1UnderhandText;
    [SerializeField] private TextMeshProUGUI P2UnderhandText;

    [SerializeField] PlayerInformationManager Player1Info;
    [SerializeField] PlayerInformationManager Player2Info;

    void Start()
    {
        P1NameHeader.text = Player1Info.Info.name;
        P2NameHeader.text = Player2Info.Info.name;

        P1ScoreText.text = Player1Info.Info.score.ToString();
        P2ScoreText.text = Player2Info.Info.score.ToString();
        if (Player1Info.Info.score > Player2Info.Info.score)
            P1ScoreText.color = Color.yellow;
        else if (Player1Info.Info.score < Player2Info.Info.score)
            P2ScoreText.color = Color.yellow;

        P1SmashText.text = Player1Info.Info.smashCount.ToString();
        P2SmashText.text = Player2Info.Info.smashCount.ToString();
        if (Player1Info.Info.smashCount > Player2Info.Info.smashCount)
            P1SmashText.color = Color.yellow;
        else if (Player1Info.Info.smashCount < Player2Info.Info.smashCount)
            P2SmashText.color = Color.yellow;

        P1DefenseText.text = Player1Info.Info.defenceCount.ToString();
        P2DefenseText.text = Player2Info.Info.defenceCount.ToString();
        if (Player1Info.Info.defenceCount > Player2Info.Info.defenceCount)
            P1DefenseText.color = Color.yellow;
        else if (Player1Info.Info.defenceCount < Player2Info.Info.defenceCount)
            P2DefenseText.color = Color.yellow;

        P1OverhandText.text = Player1Info.Info.overhandCount.ToString();
        P2OverhandText.text = Player2Info.Info.overhandCount.ToString();

        if (Player1Info.Info.overhandCount > Player2Info.Info.overhandCount)
            P1OverhandText.color = Color.yellow;
        else if (Player1Info.Info.overhandCount < Player2Info.Info.overhandCount)
            P2OverhandText.color = Color.yellow;

        P1UnderhandText.text = Player1Info.Info.underhandCount.ToString();
        P2UnderhandText.text = Player2Info.Info.underhandCount.ToString();
        if (Player1Info.Info.underhandCount > Player2Info.Info.underhandCount)
            P1UnderhandText.color = Color.yellow;
        else if (Player1Info.Info.underhandCount < Player2Info.Info.underhandCount)
            P2UnderhandText.color = Color.yellow;

        winText.text = ((GameManager.instance.Winner == GameManager.Players.Player1) ? Player1Info.Info.name : Player2Info.Info.name) + " Win!";

    }



    // Update is called once per frame
    void Update()
    {
    }
}
