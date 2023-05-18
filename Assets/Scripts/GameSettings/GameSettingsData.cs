using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettingsData
{
    [SerializeField]
    public long lastUpdated;

    // General


    // Audio
    [SerializeField] public float MasterVolume;
    [SerializeField] public float MusicVolume;
    [SerializeField] public float SoundVolume;
    [SerializeField] public float UIVolume;

    // Video
    [SerializeField] public bool fullscreen;
    [SerializeField] public int resolutionWidth;
    [SerializeField] public int resolutionHeight;

    // Multiplayer
    [SerializeField] public string multiplayerName;

    public GameSettingsData()
    {

        MasterVolume = 1.0f;
        MusicVolume  = 1.0f;
        SoundVolume  = 1.0f;
        UIVolume = 1.0f;

        fullscreen = true;
        resolutionWidth = 1920;
        resolutionHeight = 1080;

        multiplayerName = "";
    }
}
