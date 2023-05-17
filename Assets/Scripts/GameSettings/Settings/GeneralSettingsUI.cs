using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GeneralSettingsUI : MonoBehaviour, ISettingsDataPersistence
{

    GameSettingsData settingsData;

    public void LoadData(GameSettingsData data)
    {
        settingsData = data;

    }

    public void SaveData(GameSettingsData data)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SaveData()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
