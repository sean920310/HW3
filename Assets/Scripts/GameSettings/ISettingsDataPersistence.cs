using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsDataPersistence
{
    void LoadData(GameSettingsData data);

    // The 'ref' keyword was removed from here as it is not needed.
    // In C#, non-primitive types are automatically passed by reference.
    void SaveData(GameSettingsData data);
}