using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsSettings : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] InputActionReference moveLeftActionRef;
    [SerializeField] TextMeshProUGUI moveLeftActionText;

    [SerializeField] InputActionReference moveRightActionRef;
    [SerializeField] TextMeshProUGUI moveRightActionText;

    [SerializeField] InputActionReference jumpActionRef;
    [SerializeField] TextMeshProUGUI jumpActionText;

    [SerializeField] InputActionReference jumpDownActionRef;
    [SerializeField] TextMeshProUGUI jumpDownActionText;

    [SerializeField] InputActionReference crouchActionRef;
    [SerializeField] TextMeshProUGUI crouchActionText;

    [SerializeField] InputActionReference mainWeaponActionRef;
    [SerializeField] TextMeshProUGUI mainWeaponActionText;

    [SerializeField] InputActionReference secondaryWeaponActionRef;
    [SerializeField] TextMeshProUGUI secondaryWeaponActionText;

    public void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();

        LoadBinding();
        UpdateKeyText();
    }

    public void RebindingMoveLeft()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        moveLeftActionText.text = "Wait For Input...";

        var axis = moveLeftActionRef.action.ChangeCompositeBinding("1DAxis");
        var negative = axis.NextPartBinding("Negative");

        moveLeftActionRef.action.PerformInteractiveRebinding(negative.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                moveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(negative.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                moveLeftActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingMoveRight()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        moveRightActionText.text = "Wait For Input...";

        var axis = moveLeftActionRef.action.ChangeCompositeBinding("1D Axis");
        var positive = axis.NextPartBinding("Positive");

        moveRightActionRef.action.PerformInteractiveRebinding(positive.bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                moveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.GetBindingDisplayString(positive.bindingIndex);
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                moveRightActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingJump()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        jumpActionText.text = "Wait For Input...";
        jumpActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                jumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                jumpActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingJumpDown()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        jumpDownActionText.text = "Wait For Input...";
        jumpDownActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                jumpDownActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                jumpDownActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingCrouchDown()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        crouchActionText.text = "Wait For Input...";
        crouchActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                crouchActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                crouchActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingMainWeapon()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        mainWeaponActionText.text = "Wait For Input...";
        mainWeaponActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                mainWeaponActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                mainWeaponActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
        SaveBinding();
    }
    public void RebindingSecondaryWeapon()
    {
        // Rebinding
        playerInput.SwitchCurrentActionMap("PlayerRebinding");

        secondaryWeaponActionText.text = "Wait For Input...";
        secondaryWeaponActionRef.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                secondaryWeaponActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .OnCancel(operation =>
            {
                var path = operation.action.bindings[0].effectivePath;
                var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                secondaryWeaponActionText.text = keyStr;
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();

        SaveBinding();
    }

    public void LoadBinding()
    {
        string bindingData = PlayerPrefs.GetString("binding");
        if(bindingData != "")
        {
            playerInput.actions.LoadBindingOverridesFromJson(bindingData);
        }
        else
        {
            Debug.LogWarning("No Key Binding Data");
        }
    }

    public void UpdateKeyText()
    {
        var axis = moveLeftActionRef.action.ChangeCompositeBinding("1DAxis");

        var negative = axis.NextPartBinding("Negative");
        var path = moveLeftActionRef.action.GetBindingDisplayString(negative.bindingIndex);
        var keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        moveLeftActionText.text = keyStr;

        var positive = axis.NextPartBinding("Positive");
        path = moveRightActionRef.action.GetBindingDisplayString(positive.bindingIndex);
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        moveRightActionText.text = keyStr;

        path = jumpActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        jumpActionText.text = keyStr;

        path = jumpDownActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        jumpDownActionText.text = keyStr;

        path = crouchActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        crouchActionText.text = keyStr;

        path = mainWeaponActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        mainWeaponActionText.text = keyStr;

        path = secondaryWeaponActionRef.action.bindings[0].effectivePath;
        keyStr = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        secondaryWeaponActionText.text = keyStr;

    }

    public void OnResetClick()
    {
        PlayerPrefs.SetString("binding", "");
        string bindingData = PlayerPrefs.GetString("binding");
        playerInput.actions.LoadBindingOverridesFromJson(bindingData);
        UpdateKeyText();
    }
    public void SaveBinding()
    {
        string allActionsBindingData = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("binding", allActionsBindingData);
        PlayerPrefs.Save();
    }
}
