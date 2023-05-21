using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour, ISettingsDataPersistence
{
    [SerializeField] Slider MasterVolume;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider SoundVolume;
    [SerializeField] Slider UIVolume;

    [SerializeField] AudioMixer audioMixer;

    GameSettingsData settingsData;

    public void LoadData(GameSettingsData data)
    {
        settingsData = data;

        if(MasterVolume)
            MasterVolume.value = data.MasterVolume;

        if (MusicVolume)
            MusicVolume.value = data.MusicVolume;

        if (SoundVolume)
            SoundVolume.value = data.SoundVolume;

        if(UIVolume)
            UIVolume.value = data.UIVolume;


        audioMixer.SetFloat("Master", MasterVolume.value);
        audioMixer.SetFloat("Music", MusicVolume.value);
        audioMixer.SetFloat("Sound", SoundVolume.value);
        audioMixer.SetFloat("UI", UIVolume.value);
    }

    public void SaveData(GameSettingsData data)
    {
        if (MasterVolume)
            data.MasterVolume = MasterVolume.value;

        if (MusicVolume)
            data.MusicVolume = MusicVolume.value;

        if (SoundVolume)
            data.SoundVolume = SoundVolume.value;

        if (UIVolume)
            data.UIVolume = UIVolume.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        MasterVolume.onValueChanged.AddListener(delegate {
            SaveMasterVolumeData();
        });
        MusicVolume.onValueChanged.AddListener(delegate {
            SaveMusicVolumeData();
        });
        SoundVolume.onValueChanged.AddListener(delegate {
            SaveSoundVolumeData();
        });
        UIVolume.onValueChanged.AddListener(delegate {
            SaveUIVolumeData();
        });
    }

    private void SaveUIVolumeData()
    {
        if (settingsData != null)
            settingsData.UIVolume = UIVolume.value;
        audioMixer.SetFloat("UI", UIVolume.value);
    }

    private void SaveSoundVolumeData()
    {
        if (settingsData != null)
            settingsData.SoundVolume = SoundVolume.value;
        audioMixer.SetFloat("Sound", SoundVolume.value);
    }

    private void SaveMusicVolumeData()
    {
        if (settingsData != null)
            settingsData.MusicVolume = MusicVolume.value;
        audioMixer.SetFloat("Music", MusicVolume.value);
    }

    private void SaveMasterVolumeData()
    {
        if (settingsData != null)
            settingsData.MasterVolume = MasterVolume.value;
        audioMixer.SetFloat("Master", MasterVolume.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
