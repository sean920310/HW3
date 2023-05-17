using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private LoadingScene loadingScene;
    public void onRematchBTNClick()
    {
        loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void onQuitBTNClick()
    {
        loadingScene.LoadScene(0);
    }
}
