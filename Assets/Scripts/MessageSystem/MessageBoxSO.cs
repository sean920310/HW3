using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MessageBoxData
{
    public string HintText;
    public string ActionHintText;
    public ToturialManager.ToturialStates toturialStates;
}

[CreateAssetMenu]
public class MessageBoxSO : ScriptableObject
{
    public MessageBoxData messageBoxData;
}
