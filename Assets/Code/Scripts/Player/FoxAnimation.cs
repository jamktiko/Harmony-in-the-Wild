using System;
using System.Collections.Generic;
using UnityEngine;
public class FoxAnimation : MonoBehaviour
{
    internal Animator animator;
    public bool MovementRestricted => Restrictors.Count > 0;
    public List<object> Restrictors = new List<object>();
    internal float speed;
    private FoxAnimationState _currentState;
    private FoxAnimationState _requestedState;

    // Using a cached hash is more efficient than using a string
    private readonly static int _stateHash = Animator.StringToHash("State"); // int
    private readonly static int _statePreviousHash = Animator.StringToHash("PreviousState"); // int
    private readonly static int _setStateHash = Animator.StringToHash("SetState"); // trigger

    private readonly static int _vertMoveHash = Animator.StringToHash("VerticalMove"); // float
    private readonly static int _horMoveHash = Animator.StringToHash("HorizontalMove"); // float
    private readonly static int _upMoveHash = Animator.StringToHash("UpMove"); // float

    private readonly static int _jumpHash = Animator.StringToHash("Jump"); // trigger
    private readonly static int _headButtHash = Animator.StringToHash("HeadButt"); // trigger
    private readonly static int _freezingHash = Animator.StringToHash("Freezing"); // Trigger
    private readonly static int _snowDiveHash = Animator.StringToHash("SnowDive"); // Trigger
    private readonly static int _collectingFromBushHash = Animator.StringToHash("CollectingFromBush"); // Trigger
    private readonly static int _collectingFromGroundHash = Animator.StringToHash("CollectingFromGround"); // Trigger

    private readonly static int _isGroundedHash = Animator.StringToHash("isGrounded"); // Bool
    private readonly static int _isChargingJumpHash = Animator.StringToHash("isChargingJump"); // Bool
    private readonly static int _isSittingHash = Animator.StringToHash("isSitting"); // Bool
    private readonly static int _isLayingHash = Animator.StringToHash("isLaying"); // Bool
    private readonly static int _isGlidingHash = Animator.StringToHash("isGliding"); // Bool

    // Are these used?
    private readonly static int _isJumpingHash = Animator.StringToHash("isJumping"); // Bool
    private readonly static int _goingLeftHash = Animator.StringToHash("GoingLeft"); // Bool
    private readonly static int _isSwimmingHash = Animator.StringToHash("isSwimming"); // Bool
    private readonly static int _isReadyToShake = Animator.StringToHash("isReadyToShake"); // Bool
    private readonly static int _isReadyToSwim = Animator.StringToHash("isReadyToSwim"); // Bool
    private readonly static int _isSnowDivingHash = Animator.StringToHash("isSnowDiving"); // Bool
    private readonly static int _isFreezingHash = Animator.StringToHash("isFreezing"); // Bool

    private List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();

    #region Unity Callbacks
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        foreach (AnimatorControllerParameter item in animator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                animatorBools.Add(item);
            }
        }
    }

    private void Update()
    {
        if (_currentState == _requestedState)
        {
            return;
        }
        animator.SetInteger(_stateHash, (int)_requestedState);
        animator.SetInteger(_statePreviousHash, (int)_currentState);
        animator.SetTrigger(_setStateHash);
        _currentState = _requestedState;
    }
    #endregion

    /// <summary>
    /// Drives animator vars. State, PreviousState and SetState
    /// </summary>
    internal FoxAnimationState State { get => _currentState; set => _requestedState = value; }

    // Locomotion(0) state specific variables
    internal float HorizontalMove { get => animator.GetFloat(_horMoveHash); set => animator.SetFloat(_horMoveHash, value * (Sprinting ? 2f : 1f)); }
    internal float VerticalMove { get => animator.GetFloat(_vertMoveHash); set => animator.SetFloat(_vertMoveHash, value * (Sprinting ? 2f : 1f)); }
    internal float UpMove { get => animator.GetFloat(_upMoveHash); set => animator.SetFloat(_upMoveHash, value); }
    public bool Sprinting { get; internal set; } // Multiplies the movement values
    public void SetMovement(float horizontalInput, float verticalInput)
    {
        animator.SetFloat(_horMoveHash, horizontalInput * (Sprinting ? 2f : 1f));
        animator.SetFloat(_vertMoveHash, verticalInput * (Sprinting ? 2f : 1f));
    }
    public void SetMovement(float horizontalInput, float verticalInput, float dampTime, float deltaTime)
    {
        animator.SetFloat(_horMoveHash, horizontalInput * (Sprinting ? 2f : 1f), dampTime, deltaTime);
        animator.SetFloat(_vertMoveHash, verticalInput * (Sprinting ? 2f : 1f), dampTime, deltaTime);
    }


    // Resting(2) state specific variables
    public bool Sitting { get => animator.GetBool(_isSittingHash); internal set => animator.SetTrigger(_isSittingHash); }
    public bool Laying { get => animator.GetBool(_isLayingHash); internal set => animator.SetTrigger(_isLayingHash); }

    // Actions
    internal void Jump() => animator.SetTrigger(_jumpHash);
    internal void ChargingJump(bool v) => animator.SetBool(_isChargingJumpHash, v);
    internal void HeadButt() => animator.SetTrigger(_headButtHash);
    internal void Freezing() => animator.SetTrigger(_freezingHash);
    internal void SnowDive() => animator.SetTrigger(_snowDiveHash);
    internal void CollectFromBush() => animator.SetTrigger(_collectingFromBushHash);
    internal void CollectFromGround() => animator.SetTrigger(_collectingFromGroundHash);

    // Conditions
    public bool IsGrounded { get => animator.GetBool(_isGroundedHash); internal set => animator.SetBool(_isGroundedHash, value); }
    public bool IsGliding { get => animator.GetBool(_isGlidingHash); internal set => animator.SetBool(_isGlidingHash, value); }
    public bool IsChargingJump { get => animator.GetBool(_isChargingJumpHash); internal set => animator.SetBool(_isChargingJumpHash, value); }
    public bool ReadyToSwim { get => animator.GetBool(_isReadyToSwim); internal set => animator.SetBool(_isReadyToSwim, value); }

    #region Backwards compatibility
    internal void SetBool(Parameter parameter, bool v) => animator.SetBool(ParameterToHash(parameter), v);
    internal bool GetBool(Parameter parameter) => animator.GetBool(ParameterToHash(parameter));
    internal void SetFloat(Parameter parameter, float value) => animator.SetFloat(ParameterToHash(parameter), value);
    internal void SetFloat(Parameter parameter, float value, float dampTime, float deltaTime) => animator.SetFloat(ParameterToHash(parameter), value, dampTime, deltaTime);
    internal void SetTrigger(Parameter parameter) => animator.SetTrigger(ParameterToHash(parameter));

    [Obsolete("This is for backwards compatibility reasons and will be removed.")]
    internal void SetBool(string boolName, bool v)
    {
        Debug.LogWarning("Using Animator with string");
        animator.SetBool(boolName, v);
    }

    private static int ParameterToHash(Parameter parameter)
    {
        return parameter switch
        {
            Parameter.state => _stateHash,
            Parameter.statePrevious => _statePreviousHash,
            Parameter.setState => _setStateHash,
            Parameter.vertMove => _vertMoveHash, // Still needed by SetFloat
            Parameter.horMove => _horMoveHash, // Still needed by SetFloat
            Parameter.upMove => _upMoveHash, // Still needed by SetFloat
            Parameter.jump => _jumpHash,
            Parameter.doHeadButt => _headButtHash,
            Parameter.freezing => _freezingHash,
            Parameter.snowDive => _snowDiveHash,
            Parameter.collectingBerry => _collectingFromBushHash,
            Parameter.collectingPinecone => _collectingFromGroundHash,
            Parameter.isGrounded => _isGroundedHash,
            Parameter.isChargingJump => _isChargingJumpHash,
            Parameter.isSitting => _isSittingHash,
            Parameter.isLaying => _isLayingHash,
            Parameter.isGliding => _isGlidingHash,
            Parameter.isJumping => _isJumpingHash,
            Parameter.isSwimming => _isSwimmingHash,
            Parameter.isReadyToSwim => _isReadyToSwim,
            Parameter.isReadyToShake => _isReadyToShake,
            Parameter.isSnowDiving => _isSnowDivingHash,
            Parameter.isFreezing => _isFreezingHash,
            Parameter.goingLeft => _goingLeftHash,
            _ => throw new NotImplementedException("ParameterToHash " + parameter.ToString()),
        };
    }

    [Obsolete("This is for backwards compatibility reasons and will be removed.")]
    public enum Parameter
    {
        state,
        statePrevious,
        setState,
        horMove,
        vertMove,
        upMove,
        jump,
        doHeadButt,
        freezing,
        snowDive,
        collectingBerry,
        collectingPinecone,
        isGrounded,
        isChargingJump,
        isSitting,
        isLaying,
        isGliding,
        isJumping,
        isSwimming,
        isReadyToSwim,
        isReadyToShake,
        isSnowDiving,
        isFreezing,
        goingLeft
    }
    #endregion

}

public enum FoxAnimationState
{
    Default = 0,
    Swimming = 1,
    Rest = 2
}