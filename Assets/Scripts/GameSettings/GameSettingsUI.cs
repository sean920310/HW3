using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsUI : MonoBehaviour
{
    private bool isOpen = false;

    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject VideoPanel;
    [SerializeField] GameObject MultiplayerPanel;
    private void Update()
    {
        //if(gameObject.activeSelf)
        //{
        //    isOpen = true;
        //}
        //else
        //{
        //    if (isOpen)
        //        GameSettingsManager.instance.SaveSettings();

        //    isOpen = false;
        //}
    }

    public void onGeneralBTNClick()
    {
        GeneralPanel.SetActive(true);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(false);
        MultiplayerPanel.SetActive(false);
    }
    public void onAudioBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(false);
        MultiplayerPanel.SetActive(false);
    }
    public void onControlsBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(true);
        VideoPanel.SetActive(false);
        MultiplayerPanel.SetActive(false);
    }
    public void onVideoBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(true);
        MultiplayerPanel.SetActive(false);
    }
    public void onMultiplayerBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(false);
        MultiplayerPanel.SetActive(true);
    }
}
