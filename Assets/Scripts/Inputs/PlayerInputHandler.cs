using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler instance;

    public PlayerInput playerInput;
    public InputActionMap playerInputActionMap;
    public InputAction MoveInput,
        SprintInput,
        LookInput,
        SnowDiveInput,
        GlideInput,
        InteractInput,
        UseAbilityInput,
        AbilityToggleInput,
        ChargeJumpInput,
        TelegrabGrabInput,
        ChangeCameraInput,
        JumpInput,
        DialogueInput,
        CloseUIInput,
        DialogueUpInput,
        DialogueDownInput,
        SelectInput,
        OpenMapInput,
        PauseInput,
        TogglePlayerModelInput,
        DebugSaveInput,
        DebugDeleteSaveInput,
        DebugDeleteSaveInput2,
        DebugReloadOverworldInput,
        DebugReloadMainMenuInput,
        DebugReloadCurrentSceneInput,
        DebugDevToolsInput,
        DebugAbilitiesCheckOne,
        DebugAbilitiesCheckAll,
        DebugAbilitiesUnlockOne,
        DebugAbilitiesUnlockAll,
        DebugVegetationColorChanger,
        DebugTrailerCameraToggle,
        DebugHideUI,
        DebugIncreaseTrailerCameraSpeed,
        DebugDecreaseTrailerCameraSpeed;
    

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        AssignInputs();
    }
    private void AssignInputs() 
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputActionMap = playerInput.actions.FindActionMap("Player");
        MoveInput = playerInput.actions.FindAction("Move");
        SprintInput = playerInput.actions.FindAction("Sprint");
        LookInput = playerInput.actions.FindAction("Look");
        JumpInput = playerInput.actions.FindAction("Jump");
        SnowDiveInput = playerInput.actions.FindAction("SnowDive");
        GlideInput = playerInput.actions.FindAction("Glide");
        InteractInput = playerInput.actions.FindAction("Interact");
        UseAbilityInput = playerInput.actions.FindAction("UseAbility");
        AbilityToggleInput = playerInput.actions.FindAction("AbilityToggle");
        ChargeJumpInput = playerInput.actions.FindAction("ChargeJump");
        TelegrabGrabInput = playerInput.actions.FindAction("TelegrabGrab");
        ChangeCameraInput = playerInput.actions.FindAction("ChangeCamera");
        DialogueInput = playerInput.actions.FindAction("DialogueNext");
        CloseUIInput = playerInput.actions.FindAction("Close");
        SelectInput = playerInput.actions.FindAction("Select");
        DialogueUpInput = playerInput.actions.FindAction("DialogueUp");
        DialogueDownInput = playerInput.actions.FindAction("DialogueDown");
        OpenMapInput = playerInput.actions.FindAction("OpenMap");
        PauseInput = playerInput.actions.FindAction("Pause");
        TogglePlayerModelInput= playerInput.actions.FindAction("TogglePlayerModel");
        DebugSaveInput = playerInput.actions.FindAction("DebugSave");
        DebugDeleteSaveInput = playerInput.actions.FindAction("DebugDeleteSave");
        DebugDeleteSaveInput2 = playerInput.actions.FindAction("DebugDeleteSave2");
        DebugReloadOverworldInput = playerInput.actions.FindAction("DebugReloadOverworld");
        DebugReloadMainMenuInput = playerInput.actions.FindAction("DebugReloadMainMenu");
        DebugReloadCurrentSceneInput = playerInput.actions.FindAction("DebugReloadCurrentScene");
        DebugDevToolsInput= playerInput.actions.FindAction("DebugDevTools");
        DebugAbilitiesCheckOne = playerInput.actions.FindAction("DebugAbilitiesCheckOne");
        DebugAbilitiesCheckAll = playerInput.actions.FindAction("DebugAbilitiesCheckAll");
        DebugAbilitiesUnlockOne = playerInput.actions.FindAction("DebugAbilitiesUnlockOne");
        DebugAbilitiesUnlockAll = playerInput.actions.FindAction("DebugAbilitiesUnlockAll");
        DebugVegetationColorChanger = playerInput.actions.FindAction("DebugVegetationColorChanger");
        DebugTrailerCameraToggle = playerInput.actions.FindAction("DebugTrailerCameraToggle");
        DebugHideUI = playerInput.actions.FindAction("DebugHideUI");
        DebugIncreaseTrailerCameraSpeed = playerInput.actions.FindAction("DebugIncreaseTrailerCameraSpeed");
        DebugDecreaseTrailerCameraSpeed = playerInput.actions.FindAction("DebugDecreaseTrailerCameraSpeed");
    }
}
