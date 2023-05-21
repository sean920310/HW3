using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GeneralSettingsUI : MonoBehaviour, ISettingsDataPersistence
{

    GameSettingsData settingsData;
    [SerializeField] public UnityEngine.UI.Toggle TutorialToggle;
    [SerializeField] public UnityEngine.UI.Toggle SingleplayerMobileToggle;


    public void LoadData(GameSettingsData data)
    {
        settingsData = data;
        TutorialToggle.isOn = data.tutorial;
        SingleplayerMobileToggle.isOn = data.singleplayerMobile;
    }

    public void SaveData(GameSettingsData data)
    {
        data.tutorial = TutorialToggle.isOn;
        data.singleplayerMobile = SingleplayerMobileToggle.isOn;
    }


    // Start is called before the first frame update
    void Start()
    {
        //TutorialToggle.onValueChanged.AddListener(delegate {
        //    GameSettingsManager.instance.settingsData.tutorial = TutorialToggle.isOn;
        //});

        //SingleplayerMobileToggle.onValueChanged.AddListener(delegate {
        //    GameSettingsManager.instance.settingsData.singleplayerMobile = SingleplayerMobileToggle.isOn;
        //});

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
