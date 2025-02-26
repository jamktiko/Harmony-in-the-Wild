using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance;

    [FormerlySerializedAs("playerInput")] public PlayerInput PlayerInput;
    [FormerlySerializedAs("playerInputActionMap")] public InputActionMap PlayerInputActionMap;

    //input actions
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
        CinematicCamera,
        LayInput,
        SitInput,
        SwitchTimeScale;

    //debug input actions
#if DEBUG
    public InputAction DebugSaveInput,
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
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        AssignInputs();
    }
    private void AssignInputs()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInputActionMap = PlayerInput.actions.FindActionMap("Player");
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
        OpenMapInput = PlayerInput.actions.FindAction("OpenMap");
        PauseInput = PlayerInput.actions.FindAction("Pause");
        TogglePlayerModelInput = PlayerInput.actions.FindAction("TogglePlayerModel");
        CinematicCamera = PlayerInput.actions.FindAction("CinematicCamera");
        SwitchTimeScale = PlayerInput.actions.FindAction("SwitchTimeScale");
        LayInput = PlayerInput.actions.FindAction("Lay");
        SitInput = PlayerInput.actions.FindAction("Sit");

        //debug inputs
#if DEBUG
        DebugSaveInput = PlayerInput.actions.FindAction("DebugSave");
        DebugDeleteSaveInput = PlayerInput.actions.FindAction("DebugDeleteSave");
        DebugDeleteSaveInput2 = PlayerInput.actions.FindAction("DebugDeleteSave2");
        DebugReloadOverworldInput = PlayerInput.actions.FindAction("DebugReloadOverworld");
        DebugReloadMainMenuInput = PlayerInput.actions.FindAction("DebugReloadMainMenu");
        DebugReloadCurrentSceneInput = PlayerInput.actions.FindAction("DebugReloadCurrentScene");
        DebugDevToolsInput = PlayerInput.actions.FindAction("DebugDevTools");
        DebugAbilitiesCheckOne = PlayerInput.actions.FindAction("DebugAbilitiesCheckOne");
        DebugAbilitiesCheckAll = PlayerInput.actions.FindAction("DebugAbilitiesCheckAll");
        DebugAbilitiesUnlockOne = PlayerInput.actions.FindAction("DebugAbilitiesUnlockOne");
        DebugAbilitiesUnlockAll = PlayerInput.actions.FindAction("DebugAbilitiesUnlockAll");
        DebugVegetationColorChanger = PlayerInput.actions.FindAction("DebugVegetationColorChanger");
        DebugTrailerCameraToggle = PlayerInput.actions.FindAction("DebugTrailerCameraToggle");
        DebugHideUI = PlayerInput.actions.FindAction("DebugHideUI");
        DebugIncreaseTrailerCameraSpeed = PlayerInput.actions.FindAction("DebugIncreaseTrailerCameraSpeed");
        DebugDecreaseTrailerCameraSpeed = PlayerInput.actions.FindAction("DebugDecreaseTrailerCameraSpeed");
#endif
    }
}
