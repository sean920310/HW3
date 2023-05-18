using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsUI : MonoBehaviour
{
    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject VideoPanel;
    [SerializeField] GameObject MultiplayerPanel;

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
