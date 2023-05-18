using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GeneralSettingsUI : MonoBehaviour, ISettingsDataPersistence
{

    GameSettingsData settingsData;
    [SerializeField] public UnityEngine.UI.Toggle TutorialToggle;


    public void LoadData(GameSettingsData data)
    {
        settingsData = data;
        TutorialToggle.isOn = data.tutorial;
    }

    public void SaveData(GameSettingsData data)
    {
        data.tutorial = TutorialToggle.isOn;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
