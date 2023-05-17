using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GeneralSettingsUI : MonoBehaviour, ISettingsDataPersistence
{
    [SerializeField] UnityEngine.UI.Toggle MinimapToggle;

    GameSettingsData settingsData;

    public void LoadData(GameSettingsData data)
    {
        settingsData = data;
        MinimapToggle.isOn = data.useMinimap;
    }

    public void SaveData(GameSettingsData data)
    {
        data.useMinimap = MinimapToggle.isOn;
    }

    // Start is called before the first frame update
    void Start()
    {

        MinimapToggle.onValueChanged.AddListener(delegate {
            SaveData();
        });
    }

    public void SaveData()
    {
        if (settingsData != null)
            settingsData.useMinimap = MinimapToggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
