using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private LoadingScene loadingScene;
    public void onRematchBTNClick()
    {
        loadingScene.LoadScene(1);
    }
    public void onQuitBTNClick()
    {
        loadingScene.LoadScene(0);
    }
}
