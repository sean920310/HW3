using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform settingPanel;
    [SerializeField] public LoadingScene loadingScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.gameObject.SetActive(false);
        }
    }

    public void onRematchBTNClick()
    {
        loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void onQuitBTNClick()
    {
        loadingScene.LoadScene(0);
    }
    public void onSettingsBTNClick()
    {
        settingPanel.gameObject.SetActive(true);
    }
}
