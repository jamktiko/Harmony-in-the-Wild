using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
    private PlayerInputActions playerInputActions;
    [Header("Input reader object")]
    [SerializeField] InputReader _inputReader;
    private void OnEnable()
    {
        if (playerInputActions==null)
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
}
