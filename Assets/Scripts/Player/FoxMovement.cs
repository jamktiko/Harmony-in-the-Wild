using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxMovement : MonoBehaviour
{
    public static FoxMovement instance;

    [Header("Movement")]
    public CameraMovement cameraMovement;
    public Rigidbody rb;
    public float moveSpeed = 7f;
    public float sprintSpeed = 12f;

    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag = 5f;

    private bool isSprinting;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    private Vector3 moveDirection;

    [Header("Slopes")]
    public RaycastHit hit3;
    [SerializeField] private float playerHeight;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float jumpCooldown = 1f;

    [HideInInspector] public bool isReadyToJump = true;

    [Header("Checks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask waterLayerMask;
    public LayerMask climbWallLayerMask;
    [SerializeField] private LayerMask snowLayerMask;
    public LayerMask moveableLayerMask;
    public Transform cameraPosition;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private Transform foxMiddle;
    [SerializeField] private Transform foxBottom;
    //[SerializeField] private Transform fox;
    //[SerializeField] private Transform arcticFox;

    private float viewDistance = 50f;
    private Vector3 boxSize = new Vector3(0f, 2f, 2f);

    private AbilityCycle abilityCycle;
    private bool isLoaded;

    [Header("Animations")]
    public Animator playerAnimator;
    private List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();
    private void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetSceneByBuildIndex(3).isLoaded)
        {
            LoadPlayerPosition();
        }
    }
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetSceneByBuildIndex(3).isLoaded)
        {
            LoadPlayerPosition();
        }

        rb.freezeRotation = true;
        abilityCycle = GetComponent<AbilityCycle>();
        playerAnimator = GetComponentInChildren<Animator>();

        foreach (AnimatorControllerParameter item in playerAnimator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                animatorBools.Add(item);
            }
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) && !isLoaded)
        {
            LoadPlayerPosition();

            isLoaded = true;
        }

        SpeedControl();
        IsOnSlope();
        Animations();

        if (!DialogueManager.instance.isDialoguePlaying)
        {
            ProcessInput();
        }
    }
    private void FixedUpdate()
    {
        if (!DialogueManager.instance.isDialoguePlaying)
        {
            MovePlayer();
        }
    }
    #region INPUTS
    private void ProcessInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        SprintInput();
        JumpInput();
        AbilityInputs();
        SelectAbility();
    }

    void AbilityInputs()
    {
        //gliding
        if (Input.GetButtonDown("Jump") && !IsGrounded() && !IsInWater())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.Gliding);
        }

        //swimming
        AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.Swimming);

        //chargejumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.ChargeJumping);
        }

        //snowdiving
        if (Input.GetKey(KeyCode.LeftControl) && IsInSnow())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.SnowDiving);
        }

        //telegrabbing
        if (Input.GetMouseButtonDown(0))
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.TeleGrabbing);
        }
    }
    void SelectAbility()
    {
        //chargejump
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.ChargeJumping, out bool isChargeJumpSelected);
        if (Input.GetKeyDown(KeyCode.F) && isChargeJumpSelected)
        {
            ChargeJumping.instance.isChargeJumpActivated = !ChargeJumping.instance.isChargeJumpActivated;
        }
        if (!isChargeJumpSelected)
        {
            ChargeJumping.instance.isChargeJumpActivated = false;
        }

        //telegrab
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.TeleGrabbing, out bool isTelegrabSelected);
        if (Input.GetKeyDown(KeyCode.F) && isTelegrabSelected)
        {
            TeleGrabbing.instance.ActivateTelegrabCamera();
            TeleGrabbing.instance.isTelegrabActivated = !TeleGrabbing.instance.isTelegrabActivated;
        }
        if (!isTelegrabSelected && TeleGrabbing.instance.isTelegrabActivated)
        {
            TeleGrabbing.instance.isTelegrabActivated = false;
            TeleGrabbing.instance.ActivateTelegrabCamera();
        }
    }
    private void SprintInput()
    {
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump") && isReadyToJump && IsGrounded() && !ChargeJumping.instance.isChargeJumpActivated)
        {
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    #endregion
    private void Animations()
    {
        Walk();
        Sprint();
    }
    #region MOVEMENT
    private void MovePlayer()
    {
        float speed = 0f;
        float modifier = 1f;
        AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.SnowDiving, out bool isSnowDiveUnlocked);

        SetMovementSpeed(ref speed, ref modifier, isSnowDiveUnlocked);

        rb.AddForce(moveDirection.normalized * speed * 10f * modifier, ForceMode.Force);
    }

    private void SetMovementSpeed(ref float speed, ref float modifier, bool isSnowDiveUnlocked)
    {
        //Walking
        if (IsGrounded() && !isSprinting)
        {
            speed = moveSpeed;
        }

        //Sprinting
        if (IsGrounded() && isSprinting)
        {
            speed = sprintSpeed;
        }

        //Swimming
        if (IsInWater())
        {
            speed = Swimming.instance.swimSpeed;
        }

        //In air, Gliding
        if (!IsGrounded() && !IsInWater() && Gliding.instance.isGliding)
        {
            speed = moveSpeed;
            modifier = Gliding.instance.glidingMultiplier;
        }

        //In air, NOT Gliding
        if (!IsGrounded() && !IsInWater() && !Gliding.instance.isGliding)
        {
            speed = moveSpeed;
            modifier = Gliding.instance.airMultiplier;
        }

        //Walking on snow
        if (IsInSnow() && IsGrounded() && isSnowDiveUnlocked)
        {
            speed = SnowDiving.instance.snowDiveSpeed;
        }
    }

    private void Walk()
    {
        if (IsGrounded())
        {
            rb.mass = 1f;
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (IsGrounded())
        {
            //walking animation here
            playerAnimator.speed = 1f;

            SetDefaultAnimatorValues();
        }

        if (IsGrounded() && moveDirection == Vector3.zero)
        {
            //idle animation here
            SetDefaultAnimatorValues();
        }
    }
    private void Sprint()
    {
        if (IsGrounded() && isSprinting)
        {
            //running animation here
            SetDefaultAnimatorValues();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumping animation here

        playerAnimator.SetBool("isChargingJump", false);
        playerAnimator.SetBool("isJumping", true);
    }
    private void ResetJump()
    {
        isReadyToJump = true;
    }
    #endregion
    #region CHECKS
    public bool IsGrounded()
    {
        RaycastHit hitInfo;

        return Physics.CheckSphere(foxMiddle.position, boxSize.y, groundLayerMask, QueryTriggerInteraction.Ignore) && (Physics.Raycast(foxMiddle.position, -foxMiddle.up, out hitInfo, 1.5f, groundLayerMask) || Physics.Raycast(foxBottom.position, -foxBottom.up, out hitInfo, 1.5f, groundLayerMask));
    }
    public bool IsInWater()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.y, waterLayerMask, QueryTriggerInteraction.Ignore);
    }
    public bool IsInSnow()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.y, snowLayerMask, QueryTriggerInteraction.Ignore);
    }
    public bool IsOnSlope()
    {
        return Physics.Raycast(foxMiddle.position, Vector3.down, out hit3, playerHeight * 0.5f + 0.2f) && hit3.normal != Vector3.up;
    }
    public bool HasClimbWallCollision()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.z, climbWallLayerMask, QueryTriggerInteraction.Ignore);
    }
    #endregion
    #region MISC
    private void SetDefaultAnimatorValues()
    {
        playerAnimator.SetFloat("horMove", horizontalInput);
        playerAnimator.SetFloat("vertMove", verticalInput);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isGliding", false);
        playerAnimator.SetBool("isGrounded", true);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foxMiddle.position, boxSize.y);
    }
    #endregion
    public List<float> CollectPlayerPositionForSaving()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        string overworldSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld);

        if (activeSceneName == overworldSceneName)
        {
            return new List<float> { transform.position.x, transform.position.y, transform.position.z };
        }
        else
        {
            return new List<float> { 1627f, 118f, 360f };
        }
    }

    private void LoadPlayerPosition()
    {
        transform.position = new Vector3(
            SaveManager.instance.GetLoadedPlayerPositionData()[0],
            SaveManager.instance.GetLoadedPlayerPositionData()[1],
            SaveManager.instance.GetLoadedPlayerPositionData()[2]);
    }
}