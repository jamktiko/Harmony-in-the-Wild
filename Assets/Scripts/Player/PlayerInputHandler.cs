using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;

    InputAction MoveInput,
       SprintInput,
       LookInput,
       JumpInput;

    FoxMovement foxMovement;

    private void Awake()
    {
        AssignInput();
    }
    private void AssignInput()
    {
        foxMovement = GetComponent<FoxMovement>();
        playerInput = GetComponent<PlayerInput>();
        MoveInput = playerInput.actions.FindAction("Move");
        SprintInput = playerInput.actions.FindAction("Sprint");
        LookInput = playerInput.actions.FindAction("Look");
        JumpInput = playerInput.actions.FindAction("Jump");
    }
}
