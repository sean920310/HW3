using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsSettings : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] InputActionReference p1MoveLeftActionRef;
    [SerializeField] TextMeshProUGUI p1MoveLeftActionText;

    [SerializeField] InputActionReference p1MoveRightActionRef;
    [SerializeField] TextMeshProUGUI p1MoveRightActionText;

    [SerializeField] InputActionReference p1JumpActionRef;
    [SerializeField] TextMeshProUGUI p1JumpActionText;

    [SerializeField] InputActionReference p1OverhandActionRef;
    [SerializeField] TextMeshProUGUI p1OverhandActionText;

    [SerializeField] InputActionReference p1UnderhandActionRef;
    [SerializeField] TextMeshProUGUI p1UnderhandActionText;

    [SerializeField] InputActionReference p2MoveLeftActionRef;
    [SerializeField] TextMeshProUGUI p2MoveLeftActionText;

    [SerializeField] InputActionReference p2MoveRightActionRef;
    [SerializeField] TextMeshProUGUI p2MoveRightActionText;

    [SerializeField] InputActionReference p2JumpActionRef;
    [SerializeField] TextMeshProUGUI p2JumpActionText;

    [SerializeField] InputActionReference p2OverhandActionRef;
    [SerializeField] TextMeshProUGUI p2OverhandActionText;

    [SerializeField] InputActionReference p2UnderhandActionRef;
    [SerializeField] TextMeshProUGUI p2UnderhandActionText;

    public void Start()
    {
        LoadBinding();
        UpdateKeyText();
    }

    public void OnEnable()
    {
        LoadBinding();
        UpdateKeyText();
    }
    public void OnDisable()
    {
        SaveBinding();
    }

    #region Button Event

    public void RebindingP1MoveLeft()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p1MoveLeftActionText.text = "Wait For Input...";

        var axis = p1MoveLeftActionRef.action.ChangeCompositeBinding("1DAxis");
        var negative = axis.NextPartBinding("Negative");

        p1MoveLeftActionRef.action.PerformInteractiveRebinding(negative.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1MoveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1MoveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP1MoveRight()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p1MoveRightActionText.text = "Wait For Input...";

        var axis = p1MoveLeftActionRef.action.ChangeCompositeBinding("1D Axis");
        var positive = axis.NextPartBinding("Positive");

        p1MoveRightActionRef.action.PerformInteractiveRebinding(positive.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1MoveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1MoveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP1Jump()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p1JumpActionText.text = "Wait For Input...";
        p1JumpActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1JumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1JumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP1Overhand()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p1OverhandActionText.text = "Wait For Input...";
        p1OverhandActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1OverhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1OverhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP1Underhand()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p1UnderhandActionText.text = "Wait For Input...";
        p1UnderhandActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1UnderhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1UnderhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player1");
            })
            .Start();
        SaveBinding();
    }

    public void RebindingP2MoveLeft()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p2MoveLeftActionText.text = "Wait For Input...";

        var axis = p2MoveLeftActionRef.action.ChangeCompositeBinding("1DAxis");
        var negative = axis.NextPartBinding("Negative");

        p2MoveLeftActionRef.action.PerformInteractiveRebinding(negative.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p1MoveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2MoveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP2MoveRight()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p2MoveRightActionText.text = "Wait For Input...";

        var axis = p2MoveRightActionRef.action.ChangeCompositeBinding("1D Axis");
        var positive = axis.NextPartBinding("Positive");

        p2MoveRightActionRef.action.PerformInteractiveRebinding(positive.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2MoveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2MoveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP2Jump()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p2JumpActionText.text = "Wait For Input...";
        p2JumpActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2JumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2JumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP2Overhand()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p2OverhandActionText.text = "Wait For Input...";
        p2OverhandActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2OverhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2OverhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingP2Underhand()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        p2UnderhandActionText.text = "Wait For Input...";
        p2UnderhandActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2UnderhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                p2UnderhandActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("Player2");
            })
            .Start();
        SaveBinding();
    }

    public void OnResetClick()
    {
        PlayerPrefs.SetString("binding", "");
        string bindingData = PlayerPrefs.GetString("binding");
        playerInput.actions.LoadBindingOverridesFromJson(bindingData);
        UpdateKeyText();
    }

    #endregion

    #region Save And Load
    public void LoadBinding()
    {
        string bindingData = PlayerPrefs.GetString("keybinding");
        if(bindingData != "")
        {
            playerInput.actions.LoadBindingOverridesFromJson(bindingData);
        }
        else
        {
            Debug.LogWarning("No Key Binding Data");
        }
    }
    public void SaveBinding()
    {
        string allActionsBindingData = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("keybinding", allActionsBindingData);
        PlayerPrefs.Save();
    }
    #endregion

    public void UpdateKeyText()
    {
        var axis = p1MoveLeftActionRef.action.ChangeCompositeBinding("1DAxis");

        var negative = axis.NextPartBinding("Negative");
        var path = p1MoveLeftActionRef.action.GetBindingDisplayString(negative.bindingIndex);
        var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p1MoveLeftActionText.text = keyStr;

        var positive = axis.NextPartBinding("Positive");
        path = p1MoveRightActionRef.action.GetBindingDisplayString(positive.bindingIndex);
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p1MoveRightActionText.text = keyStr;

        path = p1JumpActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p1JumpActionText.text = keyStr;

        path = p1OverhandActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p1OverhandActionText.text = keyStr;

        path = p1UnderhandActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p1UnderhandActionText.text = keyStr;


        axis = p2MoveLeftActionRef.action.ChangeCompositeBinding("1DAxis");

        negative = axis.NextPartBinding("Negative");
        path = p2MoveLeftActionRef.action.GetBindingDisplayString(negative.bindingIndex);
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p2MoveLeftActionText.text = keyStr;

        positive = axis.NextPartBinding("Positive");
        path = p2MoveRightActionRef.action.GetBindingDisplayString(positive.bindingIndex);
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p2MoveRightActionText.text = keyStr;

        path = p2JumpActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p2JumpActionText.text = keyStr;

        path = p2OverhandActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p2OverhandActionText.text = keyStr;

        path = p2UnderhandActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        p2UnderhandActionText.text = keyStr;

    }
}
