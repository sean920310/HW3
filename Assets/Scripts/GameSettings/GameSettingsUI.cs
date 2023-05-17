using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsUI : MonoBehaviour
{
    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject VideoPanel;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onGeneralBTNClick()
    {
        GeneralPanel.SetActive(true);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(false);
    }
    public void onAudioBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(false);
    }
    public void onControlsBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(true);
        VideoPanel.SetActive(false);
    }
    public void onVideoBTNClick()
    {
        GeneralPanel.SetActive(false);
        AudioPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VideoPanel.SetActive(true);
    }
}
