using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public TextMeshProUGUI progressText;

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    public void MuliplayerLoadScene(int sceneID)
    {
        StartCoroutine(MuliplayerLoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        loadingScreen.SetActive(true);
        //yield return new WaitForSeconds(2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = (int)(progressValue * 100) + " %";
            yield return null;
        }

    }

    IEnumerator MuliplayerLoadSceneAsync(int sceneID)
    {
        loadingScreen.SetActive(true);
        //yield return new WaitForSeconds(2f);

        PhotonNetwork.LoadLevel(1);

        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            float progressValue = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / 0.9f);
            progressText.text = (int)(progressValue * 100) + " %";
            yield return null;
        }

    }
}
