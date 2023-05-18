using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour, ISettingsDataPersistence
{
    [SerializeField] TMP_InputField playerNameInput;
    public void LoadData(GameSettingsData data)
    {
        playerNameInput.text = data.multiplayerName;
    }

    public void SaveData(GameSettingsData data)
    {
        data.multiplayerName = playerNameInput.text;
    }
}
