using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIInputHandler : MonoBehaviour
{
    public static UIInputHandler Instance;

    [FormerlySerializedAs("playerInput")] public PlayerInput PlayerInput;
    [FormerlySerializedAs("playerInputActionMap")] public InputActionMap PlayerInputActionMap;
    [FormerlySerializedAs("playerUIINputActionMap")] public InputActionMap PlayerUiiNputActionMap;
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
        SelectInput;

    private void Awake()
    {
        Instance = this;

        AssignInputs();
    }
    private void AssignInputs()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MoveInput = PlayerInput.actions.FindAction("Move");
        SprintInput = PlayerInput.actions.FindAction("Sprint");
        LookInput = PlayerInput.actions.FindAction("Look");
        JumpInput = PlayerInput.actions.FindAction("Jump");
        SnowDiveInput = PlayerInput.actions.FindAction("SnowDive");
        GlideInput = PlayerInput.actions.FindAction("Glide");
        InteractInput = PlayerInput.actions.FindAction("Interact");
        UseAbilityInput = PlayerInput.actions.FindAction("UseAbility");
        AbilityToggleInput = PlayerInput.actions.FindAction("AbilityToggle");
        ChargeJumpInput = PlayerInput.actions.FindAction("ChargeJump");
        TelegrabGrabInput = PlayerInput.actions.FindAction("TelegrabGrab");
        ChangeCameraInput = PlayerInput.actions.FindAction("ChangeCamera");
        DialogueInput = PlayerInput.actions.FindAction("DialogueNext");
        CloseUIInput = PlayerInput.actions.FindAction("Close");
        SelectInput = PlayerInput.actions.FindAction("Select");
        DialogueUpInput = PlayerInput.actions.FindAction("DialogueUp");
        DialogueDownInput = PlayerInput.actions.FindAction("DialogueDown");
    }
}
