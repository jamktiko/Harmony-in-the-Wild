using System;
using System.Collections.Generic;
using UnityEngine;
public class FoxAnimation : MonoBehaviour
{
    internal Animator animator;
    public bool MovementRestricted => Restrictors.Count > 0;
    public List<object> Restrictors = new List<object>();

    // Using a cached hash is more efficient than using a string
    private readonly static int horMoveHash = Animator.StringToHash("horMove");
    private readonly static int vertMoveHash = Animator.StringToHash("vertMove");
    private readonly static int isGlidingHash = Animator.StringToHash("isGliding");
    private readonly static int isGroundedHash = Animator.StringToHash("isGrounded");
    private readonly static int isSprintingHash = Animator.StringToHash("isSprinting");
    private readonly static int isSwimmingHash = Animator.StringToHash("isSwimming");
    private readonly static int isSittingHash = Animator.StringToHash("isSitting");
    private readonly static int isLayingHash = Animator.StringToHash("isLaying");
    private readonly static int upMoveHash = Animator.StringToHash("upMove");
    private readonly static int isChargingJumpHash = Animator.StringToHash("isChargingJump");
    private readonly static int isJumpingHash = Animator.StringToHash("isJumping");
    private readonly static int isFreezingHash = Animator.StringToHash("isFreezing");
    private readonly static int isSnowDivingHash = Animator.StringToHash("isSnowDiving");
    private readonly static int doHeadButtHash = Animator.StringToHash("doHeadButt");

    internal float speed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private static int ParameterToHash(Parameter parameter)
    {
        return parameter switch
        {
            Parameter.horMove => horMoveHash,
            Parameter.vertMove => vertMoveHash,
            Parameter.isGliding => isGlidingHash,
            Parameter.isGrounded => isGroundedHash,
            Parameter.isSprinting => isSprintingHash,
            Parameter.isSwimming => isSwimmingHash,
            Parameter.isSitting => isSittingHash,
            Parameter.isLaying => isLayingHash,
            Parameter.upMove => upMoveHash,
            Parameter.isChargingJump => isChargingJumpHash,
            Parameter.isJumping => isJumpingHash,
            Parameter.isFreezing => isFreezingHash,
            Parameter.isSnowDiving => isSnowDivingHash,
            Parameter.doHeadButt => doHeadButtHash,
            _ => throw new NotImplementedException("ParameterToHash " + parameter.ToString()),
        };
    }

    internal void SetBool(Parameter parameter, bool v) => animator.SetBool(ParameterToHash(parameter), v);
    internal bool GetBool(Parameter parameter) => animator.GetBool(ParameterToHash(parameter));
    internal void SetFloat(Parameter parameter, float value) => animator.SetFloat(ParameterToHash(parameter), value);
    internal void SetFloat(Parameter parameter, float verticalInput, float dampTime, float deltaTime) => animator.SetFloat(ParameterToHash(parameter), verticalInput, dampTime, deltaTime);
    internal void SetTrigger(Parameter parameter) => animator.SetTrigger(ParameterToHash(parameter));

    internal void SetBool(string boolName, bool v)
    {
        Debug.LogWarning("Using Animator ");
        animator.SetBool(boolName, v);
    }

    public enum Parameter
    {
        horMove,
        vertMove,
        isGliding,
        isGrounded,
        isSprinting,
        isSwimming,
        isSitting,
        isLaying,
        upMove,
        upMoveisChargingJump,
        upMoveisJumping,
        upMoveisGrounded,
        upMoveisSprinting,
        isChargingJump,
        isJumping,
        isFreezing,
        isSnowDiving,
        doHeadButt,
        isCollectingPinecone,
        isCollectingBerry,
        isReadyToSwim
    }
}
