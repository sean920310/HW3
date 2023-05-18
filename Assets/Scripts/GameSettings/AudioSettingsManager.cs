
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
    }
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound", volume);
    }
    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UI", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
}
