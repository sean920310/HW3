using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBoxItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HintText;
    [SerializeField] TextMeshProUGUI ActionHintText;

    public void SetText(MessageBoxData messageBoxData)
    {
        HintText.text = messageBoxData.HintText;
        ActionHintText.text = messageBoxData.ActionHintText;
    }

    public void SetText(string hintText, string actionHintText)
    {
        HintText.text = hintText;
        ActionHintText.text = actionHintText;
    }
    public void AddHintText(char str)
    {
        HintText.text += str;
    }
}
