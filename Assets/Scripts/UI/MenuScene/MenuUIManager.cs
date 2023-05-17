using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private LoadingScene loadingScene;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OptionsMenu.SetActive(false);
        }
    }



    public void onSingleplayerBTNClick()
    {
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
