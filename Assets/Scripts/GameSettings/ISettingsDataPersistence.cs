using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsDataPersistence
{
    void LoadData(GameSettingsData data);
    void SaveData(GameSettingsData data);
}