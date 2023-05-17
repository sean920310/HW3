using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour, ISettingsDataPersistence
{
    [SerializeField] UnityEngine.UI.Toggle FullScreenToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filterResolutions;

    private float currentRefreshRate;
    private int currentResolutionIdx = 0;

    GameSettingsData settingsData;

    public void LoadData(GameSettingsData data)
    {
        settingsData = data;
        FullScreenToggle.isOn = data.fullscreen;

        //if (data.fullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        //else Screen.fullScreenMode = FullScreenMode.Windowed;

        //Screen.SetResolution(data.resolutionWidth, data.resolutionHeight, true);

    }

    public void SaveData(GameSettingsData data)
    {
        data.fullscreen = FullScreenToggle.isOn;
        data.resolutionWidth = Screen.currentResolution.width;
        data.resolutionHeight = Screen.currentResolution.height;
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        filterResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filterResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filterResolutions.Count; i++)
        {
            string resolutionOption = filterResolutions[i].width + "x" + filterResolutions[i].height + " " + filterResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filterResolutions[i].width == Screen.width && filterResolutions[i].height == Screen.height)
            {
                currentResolutionIdx = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIdx;
        resolutionDropdown.RefreshShownValue();

        FullScreenToggle.onValueChanged.AddListener(delegate {
            SaveData();
        });
    }

    public void SetResolution(int resolutionIdx)
    {
        Resolution resolution = filterResolutions[resolutionIdx];
        //Screen.SetResolution(resolution.width, resolution.height, true);

        settingsData.resolutionWidth = resolution.width;
        settingsData.resolutionHeight = resolution.height;
    }

    public void SaveData()
    {
        if (settingsData != null)
            settingsData.fullscreen = FullScreenToggle.isOn;
    }

}
