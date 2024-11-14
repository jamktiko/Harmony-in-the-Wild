using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class FoxMovement : MonoBehaviour
{
    public static FoxMovement instance;

    [Header("Movement")]
    public CameraMovement cameraMovement;
    public Rigidbody rb;
    public float moveSpeed = 7f;
    public float sprintSpeed = 12f;
    private bool canMove;
    [SerializeField] private bool enableMovementOnStart;

    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag = 5f;

    private bool isSprinting;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    private Vector3 moveDirection;

    [Header("Slopes")]
    public RaycastHit SlopeHit;
    [SerializeField] private float playerHeight;
    [SerializeField] private float maxSlopeAngle;
    private bool exitingSlope;

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
    [SerializeField] public Transform foxMiddle;
    [SerializeField] public Transform foxFront;
    [SerializeField] private Transform foxBottom;
    //[SerializeField] private Transform fox;
    //[SerializeField] private Transform arcticFox;

    //private float viewDistance = 50f;
    [SerializeField]private Vector3 boxSize = new Vector3(0f, 2f, 2f);

    private AbilityCycle abilityCycle;
    private bool isLoaded;

    [Header("Animations")]
    public Animator playerAnimator;
    private List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();
    private void Awake()
    {
        if (FoxMovement.instance != null)
        {
            Debug.LogWarning("There is more than one FoxMovement in the scene!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("OverWorld"))
        {
            canMove = true;
            
            if (File.Exists(SaveManager.instance.saveFilePath))
            {
                Invoke(nameof(LoadPlayerPosition), 0.2f);
                //LoadPlayerPosition();
            }
        }

        if (enableMovementOnStart)
        {
            canMove = true;
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

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnStartDialogue += DisableMovementForDialogue;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += EnableMovementAfterDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnStartDialogue -= DisableMovementForDialogue;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= EnableMovementAfterDialogue;
    }

    void Update()
    {
        SpeedControl();
        IsOnSlope();
        Animations();

        if (!DialogueManager.instance.isDialoguePlaying && canMove)
        {
            ProcessInput();
        }
    }
    private void FixedUpdate()
    {
        if (!DialogueManager.instance.isDialoguePlaying && canMove)
        {
            MovePlayer();
        }
    }
    #region INPUTS
    private void ProcessInput()
    {
        horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
        verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        SprintInput();
        JumpInput();
        AbilityInputs();
        SelectAbility();
    }

    void AbilityInputs()
    {
        //gliding
        if (PlayerInputHandler.instance.JumpInput.WasPressedThisFrame() && !IsGrounded() && !IsInWater())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.Gliding);
        }

        //swimming
        AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.Swimming);

        //chargejumping
        if (PlayerInputHandler.instance.JumpInput.WasPressedThisFrame() && IsGrounded())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.ChargeJumping);
        }

        //snowdiving
        if (PlayerInputHandler.instance.SnowDiveInput.IsPressed() && IsInSnow())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.SnowDiving);
        }

        //telegrabbing
        if (PlayerInputHandler.instance.TelegrabGrabInput.WasPressedThisFrame())
        {
            AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.TeleGrabbing);
        }
    }
    void SelectAbility()
    {
        //chargejump
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.ChargeJumping, out bool isChargeJumpSelected);
        if (PlayerInputHandler.instance.UseAbilityInput.WasPressedThisFrame() && isChargeJumpSelected)
        {
            ChargeJumping.instance.isChargeJumpActivated = !ChargeJumping.instance.isChargeJumpActivated;
        }
        if (!isChargeJumpSelected)
        {
            ChargeJumping.instance.isChargeJumpActivated = false;
        }

        //telegrab
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.TeleGrabbing, out bool isTelegrabSelected);
        if (PlayerInputHandler.instance.UseAbilityInput.WasPressedThisFrame() && isTelegrabSelected)
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
        isSprinting = PlayerInputHandler.instance.SprintInput.IsPressed();
    }
    private void JumpInput()
    {
        if (PlayerInputHandler.instance.JumpInput.WasPressedThisFrame() && isReadyToJump && IsGrounded() && !ChargeJumping.instance.isChargeJumpActivated)
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

        //On slope
        if (IsOnSlope()&&!exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            //turn gravity off when on slope
            rb.useGravity = !IsOnSlope();
        }

        rb.AddForce(moveDirection.normalized * speed * 10f * modifier, ForceMode.Force);

        playerAnimator.SetFloat("upMove", rb.velocity.y);
    }

    private void SetMovementSpeed(ref float speed, ref float modifier, bool isSnowDiveUnlocked)
    {

        //Limit speed on Slope
        if (IsOnSlope()&&!exitingSlope)
        {
            if (rb.velocity.magnitude>moveSpeed&&!isSprinting)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            else if (rb.velocity.magnitude > sprintSpeed && isSprinting)
            {
                rb.velocity = rb.velocity.normalized * sprintSpeed;
            }
        }

        //Walking
        if (IsGrounded() && !isSprinting)
        {
            rb.useGravity = true;
            speed = moveSpeed;
        }

        //Sprinting
        if (IsGrounded() && isSprinting)
        {
            speed = sprintSpeed;
            rb.useGravity = true;
        }

        //Swimming
        if (IsInWater())
        {
            speed = Swimming.instance.swimSpeed;
            rb.useGravity = true;
        }

        //In air, Gliding
        if (!IsGrounded() && !IsInWater() && Gliding.instance.isGliding)
        {
            //rb.useGravity = true;
            speed = moveSpeed;
            modifier = Gliding.instance.glidingMultiplier;
        }

        //In air, NOT Gliding
        if (!IsGrounded() && !IsInWater() && !Gliding.instance.isGliding)
        {
            //rb.useGravity = true;
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
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumping animation here

        playerAnimator.SetBool("isChargingJump", false);
        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isGrounded", false);
    }
    private void ResetJump()
    {
        playerAnimator.SetBool("isJumping", false);
        isReadyToJump = true;
        exitingSlope = false;
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

        //return Physics.Raycast(foxMiddle.position, Vector3.down, out SlopeHit, playerHeight + 0.2f) && SlopeHit.normal != Vector3.up;

        if (Physics.Raycast(foxFront.position, Vector3.down, out SlopeHit, playerHeight *0.5f+0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            Debug.DrawRay(foxMiddle.position, Vector3.down, Color.cyan);
            return angle < maxSlopeAngle && angle != 0;
            
        }
        return false;
        
    }
    private Vector3 GetSlopeMoveDirection() 
    {
        return Vector3.ProjectOnPlane(moveDirection, SlopeHit.normal).normalized;
    }
    public bool HasClimbWallCollision()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.z, climbWallLayerMask, QueryTriggerInteraction.Ignore);
    }
    #endregion
    #region MISC
    private void SetDefaultAnimatorValues()
    {
        playerAnimator.SetFloat("horMove", horizontalInput, 0.1f, Time.deltaTime);
        playerAnimator.SetFloat("vertMove", verticalInput, 0.1f, Time.deltaTime);
        //playerAnimator.SetBool("isJumping", false);
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

    private void LoadPlayerPosition()
    {
        PositionData loadedData = SaveManager.instance.GetLoadedPlayerPosition();

        if (loadedData != null)
        {
            Vector3 pos = new Vector3(loadedData.x, loadedData.y, loadedData.z);
            Quaternion rot = new Quaternion(loadedData.rotX, loadedData.rotY, loadedData.rotZ, loadedData.rotW);
            Debug.Log($"FM Loaded playerpos data is: {loadedData}");

            gameObject.SetActive(false);
            transform.position = pos;
            transform.rotation = rot;
            gameObject.SetActive(true);
        }

        else
        {
            Debug.Log("No saved position to be loaded. Spawning to a default location.");
        }
    }

    public PositionData CollectPlayerPositionForSaving()
    {
        PositionData data = new PositionData(transform.position, orientation.transform.rotation);
        Debug.Log($"FM CollectPos Position is: {data.x}, {data.y}, {data.z}. Rotation is: {data.rotX}, {data.rotY}, {data.rotZ}");

        return data;
    }

    // prevent jumping when dialogue starts
    private void DisableMovementForDialogue()
    {
        canMove = false;
        isReadyToJump = false;
    }

    // get ready to enable jumping when dialogue has ended
    private void EnableMovementAfterDialogue()
    {
        canMove = true;
        Invoke(nameof(ResetJump), 0.3f);   
    }
}