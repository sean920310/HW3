using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private LoadingScene loadingScene;

    [SerializeField] Button SingleplayerBtn;
    [SerializeField] Button MultiplayerBtn;
    [SerializeField] UnityEngine.UI.Toggle tutorialToggle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialToggle.isOn)
        {
            MultiplayerBtn.interactable = false;
            SingleplayerBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Tutorial";
        }
        else
        {
            SingleplayerBtn.GetComponentInChildren<TextMeshProUGUI>().text = "SinglePlayer";
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OptionsMenu.SetActive(false);
        }
    }



    public void onSingleplayerBTNClick()
    {
        if(tutorialToggle.isOn)
            loadingScene.LoadScene(4);
        else
            loadingScene.LoadScene(1);
    }
    public void onMultiplayerBTNClick()
    {
        loadingScene.LoadScene(2);
    }
    public void onSettingsBTNClick()
    {
        OptionsMenu.SetActive(true);
    }
    public void onQuitBTNClick()
    {
        Application.Quit();
    }

}
