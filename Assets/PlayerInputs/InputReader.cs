using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction OnGlideEvent = delegate { };
    public event UnityAction OnJumpEvent = delegate { };
    public event UnityAction OnAbilityToggleEvent = delegate { };
    public event UnityAction OnChargeJumpEvent = delegate { };
    public event UnityAction OnChargeJumpCanceledEvent = delegate { };
    public event UnityAction OnInteractEvent = delegate { };
    public event UnityAction OnSprintEvent = delegate { };
    public event UnityAction OnSnowDiveEvent = delegate { };
    public event UnityAction OnUseAbilityEvent = delegate { };
    public event UnityAction OnTelegrabGrabEvent = delegate { };
    public event UnityAction OnChangeCameraEvent = delegate { };
    public event UnityAction OnPauseEvent = delegate { };
    public event UnityAction OnOpenMapEvent = delegate { };
    public event UnityAction OnDialogueUpEvent = delegate { };
    public event UnityAction OnDialogueDownEvent = delegate { };
    public event UnityAction OnSelectEvent = delegate { };
    public event UnityAction OnCloseEvent = delegate { };
    public event UnityAction OnDialogueNextEvent = delegate { };
    public event UnityAction OnTogglePlayerModelEvent = delegate { };
    public event UnityAction OnDebugSaveEvent = delegate { };
    public event UnityAction OnDebugDeleteSaveEvent = delegate { };
    public event UnityAction OnDebugDeleteSave2Event = delegate { };
    public event UnityAction OnDebugReloadOverworldEvent = delegate { };
    public event UnityAction OnDebugReloadMainMenuEvent = delegate { };
    public event UnityAction OnDebugReloadCurrentSceneEvent = delegate { };
    public event UnityAction OnDebugDevToolsEvent = delegate { };
    public event UnityAction OnDebugAbilitiesCheckOneEvent = delegate { };
    public event UnityAction OnDebugAbilitiesCheckAllEvent = delegate { };
    public event UnityAction OnDebugAbilitiesUnlockOneEvent = delegate { };
    public event UnityAction OnDebugAbilitiesUnlockAllEvent = delegate { };
    public event UnityAction OnDebugVegetationColorChangerEvent = delegate { };
    public event UnityAction OnDebugTrailerCameraToggleEvent = delegate { };
    public event UnityAction OnDebugHideUIEvent = delegate { };
    public event UnityAction OnDebugIncreaseTrailerCameraSpeedEvent = delegate { };
    public event UnityAction OnDebugDecreaseTrailerCameraSpeedEvent = delegate { };
    public event UnityAction OnCinematicCameraEvent = delegate { };
    public event UnityAction OnSwitchTimeScaleEvent = delegate { };
    public event UnityAction OnLayEvent = delegate { };
    public event UnityAction OnSitEvent = delegate { };
    private PlayerInputActions playerInputActions;
    [Header("Input reader object")]
    [SerializeField] InputReader _inputReader;
    private void OnEnable()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.SetCallbacks(this);
            playerInputActions.Enable();
        }
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    public void OnAbilityToggle(InputAction.CallbackContext context)
    {
        OnAbilityToggleEvent.Invoke();
    }

    public void OnChargeJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnChargeJumpEvent.Invoke();
        if (context.canceled)
            OnChargeJumpCanceledEvent.Invoke();
    }
    public void OnGlide(InputAction.CallbackContext context)
    {
        OnGlideEvent.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        OnInteractEvent.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnJumpEvent.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSnowDive(InputAction.CallbackContext context)
    {
        OnSnowDiveEvent.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        OnSprintEvent.Invoke();
    }

    public void OnTelegrabGrab(InputAction.CallbackContext context)
    {
        OnTelegrabGrabEvent.Invoke();
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        OnUseAbilityEvent.Invoke();
    }
    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        OnChangeCameraEvent.Invoke();
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        OnPauseEvent.Invoke();
    }
    public void OnOpenMap(InputAction.CallbackContext context)
    {
        OnOpenMapEvent.Invoke();
    }
    public void OnDialogueDown(InputAction.CallbackContext context)
    {
        OnDialogueDownEvent.Invoke();
    }
    public void OnDialogueUp(InputAction.CallbackContext context)
    {
        OnDialogueUpEvent.Invoke();
    }
    public void OnSelect(InputAction.CallbackContext context)
    {
        OnSelectEvent.Invoke();
    }
    public void OnClose(InputAction.CallbackContext context)
    {
        OnCloseEvent.Invoke();
    }
    public void OnDialogueNext(InputAction.CallbackContext context)
    {
        OnDialogueNextEvent.Invoke();
    }
    public void OnTogglePlayerModel(InputAction.CallbackContext context)
    {
        OnTogglePlayerModelEvent.Invoke();
    }
    public void OnDebugSave(InputAction.CallbackContext context)
    {
        OnDebugSaveEvent.Invoke();
    }
    public void OnDebugDeleteSave(InputAction.CallbackContext context)
    {
        OnDebugDeleteSaveEvent.Invoke();
    }
    public void OnDebugDeleteSave2(InputAction.CallbackContext context)
    {
        OnDebugDeleteSave2Event.Invoke();
    }
    public void OnDebugReloadOverworld(InputAction.CallbackContext context)
    {
        OnDebugReloadOverworldEvent.Invoke();
    }
    public void OnDebugReloadMainMenu(InputAction.CallbackContext context)
    {
        OnDebugReloadMainMenuEvent.Invoke();
    }
    public void OnDebugReloadCurrentScene(InputAction.CallbackContext context)
    {
        OnDebugReloadCurrentSceneEvent.Invoke();
    }
    public void OnDebugDevTools(InputAction.CallbackContext context)
    {
        OnDebugDevToolsEvent.Invoke();
    }
    public void OnDebugAbilitiesCheckOne(InputAction.CallbackContext context)
    {
        OnDebugAbilitiesCheckOneEvent.Invoke();
    }
    public void OnDebugAbilitiesCheckAll(InputAction.CallbackContext context)
    {
        OnDebugAbilitiesCheckAllEvent.Invoke();
    }
    public void OnDebugAbilitiesUnlockOne(InputAction.CallbackContext context)
    {
        OnDebugAbilitiesUnlockOneEvent.Invoke();
    }
    public void OnDebugAbilitiesUnlockAll(InputAction.CallbackContext context)
    {
        OnDebugAbilitiesUnlockAllEvent.Invoke();
    }

    public void OnDebugVegetationColorChanger(InputAction.CallbackContext context)
    {
        OnDebugVegetationColorChangerEvent.Invoke();
    }

    public void OnDebugTrailerCameraToggle(InputAction.CallbackContext context)
    {
        OnDebugTrailerCameraToggleEvent.Invoke();
    }

    public void OnDebugHideUI(InputAction.CallbackContext context)
    {
        OnDebugHideUIEvent.Invoke();
    }

    public void OnDebugIncreaseTrailerCameraSpeed(InputAction.CallbackContext context)
    {
        OnDebugIncreaseTrailerCameraSpeedEvent.Invoke();
    }

    public void OnDebugDecreaseTrailerCameraSpeed(InputAction.CallbackContext context)
    {
        OnDebugDecreaseTrailerCameraSpeedEvent.Invoke();

    }

    public void OnCinematicCamera(InputAction.CallbackContext context)
    {
        OnCinematicCameraEvent.Invoke();
    }

    public void OnSwitchTimeScale(InputAction.CallbackContext context)
    {
        OnSwitchTimeScaleEvent.Invoke();
    }
    public void OnLay(InputAction.CallbackContext context)
    {
        OnLayEvent.Invoke();
    }
    public void OnSit(InputAction.CallbackContext context)
    {
        OnSitEvent.Invoke();
    }
}
