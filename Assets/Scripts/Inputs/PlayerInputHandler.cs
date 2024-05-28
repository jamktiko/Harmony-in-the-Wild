using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler instance;

    public PlayerInput playerInput;
    public InputActionMap playerInputActionMap;
    public InputActionMap playerUIINputActionMap;
    public InputActionMap UIInputActionMap;

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
    PauseInput;

    private void Awake()
    {
        instance = this;

        AssignInputs();
    }
    private void AssignInputs() 
    {
        playerInput = GetComponent<PlayerInput>();
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
    }
}
