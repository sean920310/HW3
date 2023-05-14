using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    [SerializeField] Image p1HatImage;
    [SerializeField] Image p2HatImage;
    [SerializeField] TextMeshProUGUI p1ScoreText;
    [SerializeField] TextMeshProUGUI p2ScoreText;
    [SerializeField] TextMeshProUGUI p1NameText;
    [SerializeField] TextMeshProUGUI p2NameText;
    [SerializeField] Color MatchPointColor;
    [SerializeField] Color NormalColor;
    public bool P1IsAboutToWin = false;
    public bool P2IsAboutToWin = false;

    [SerializeField] GameObject p1ServeHint;
    [SerializeField] GameObject p2ServeHint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayerHatUpdate(Sprite Player1HatSprite, Sprite Player2HatSprite)
    {

        if (Player1HatSprite != null)
        {
            p1HatImage.color = Color.white;
            p1HatImage.sprite = Player1HatSprite;
        }
        else
            p1HatImage.color = new Color(0, 0, 0, 0);



        if (Player2HatSprite != null)
        {
            p2HatImage.color = Color.white;
            p2HatImage.sprite = Player2HatSprite;
        }
        else
            p2HatImage.color = new Color(0, 0, 0, 0);
    }   
    public void PlayerNameUpdate(string Player1Name, string Player2Name)
    {
        p1NameText.text = Player1Name;
        p2NameText.text = Player2Name;
    }
    public void ScorePanelUpdate(int Player1Score, int Player2Score)
    {
        p1ScoreText.text = Player1Score.ToString();
        p2ScoreText.text = Player2Score.ToString();

        p1ScoreText.color = P1IsAboutToWin ? MatchPointColor : NormalColor;
        p2ScoreText.color = P2IsAboutToWin ? MatchPointColor : NormalColor;
    }
    public void SetServeHint(bool Player1Serve, bool Player2Serve)
    {
        p1ServeHint.SetActive(Player1Serve);
        p2ServeHint.SetActive(Player2Serve);
    }
}
