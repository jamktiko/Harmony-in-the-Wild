using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMovement : MonoBehaviour
{
    public static FoxMovement instance;

    [Header("Movement")]
    public CameraMovement cameraMovement;
    public Rigidbody rb;
    public float moveSpeed = 7f;
    public float sprintSpeed = 12f;

    [SerializeField] private Transform orientation;
    [SerializeField] private float swimSpeed = 5f;
    [SerializeField] private float groundDrag = 5f;

    private bool isSprinting;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    [Header("Slopes")]
    public RaycastHit hit3;
    [SerializeField] private float playerHeight;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private float airMultiplier = 0.7f;
    [SerializeField] private float glidingMultiplier = 0.4f;

    private bool isReadyToJump = true;

    [Header("Checks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask waterLayerMask;
    [SerializeField] private LayerMask climbWallLayerMask;
    [SerializeField] private LayerMask snowLayerMask;
    [SerializeField] private LayerMask moveableLayerMask;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private Transform foxMiddle;
    [SerializeField] private Transform foxBottom;
    //[SerializeField] private Transform fox;
    //[SerializeField] private Transform arcticFox;

    private float viewDistance = 50f;
    private Vector3 boxSize = new Vector3(0f, 2f, 2f);

    [Header("Abilities")]
    [SerializeField] private bool isChargeJumpActivated;
    [SerializeField] private bool isTelegrabActivated;
    [SerializeField] private float snowDiveSpeed = 15f;
    [SerializeField] private float chargeJumpHeight = 24f;
    [SerializeField] private Transform grabbedObjectPosition;
    [SerializeField] private Material materialForGrabbedObject;
    [SerializeField] private GameObject telegrabUI;

    private float chargeJumpTimer;
    private bool isObjectGrabbed;
    private bool isTelegrabbing;
    private bool isGliding;
    private bool isChargingJump;
    private bool isSnowDiving;
    private AbilityCycle abilityCycle;
    private GameObject grabbedGameObject;
    private List<TelegrabObject> telegrabObjects = new List<TelegrabObject>();
    //private TelegrabObject TelegrabObject;

    [Header("Animations")]
    public Animator playerAnimator;
    private List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();

    [Header("Audio")]
    [SerializeField] private AudioSource chargeJumpAudio;
    [SerializeField] private AudioSource chargeJumpLandingAudio;
    [SerializeField] private AudioSource glidingAudio;
    [SerializeField] private AudioSource freezingAudio;
    [SerializeField] private AudioSource snowDivingAudio;
    [SerializeField] private AudioSource swimmingAudio;
    [SerializeField] private AudioSource telegrabAudio;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
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
        SpeedControl();
        IsOnSlope();

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

        GlidingInput();
        ChargeJumpInput();
        SnowDiveInput();
        TelegrabInput();

        ReleaseChargedJump();
        Telegrab();
    }
    private void SprintInput()
    {
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump") && isReadyToJump && IsGrounded() && !isChargeJumpActivated)
        {
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    #region ABILITY INPUTS
    private void GlidingInput()
    {
        AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.SnowDiving, out bool isUnlocked);
        if (Input.GetButtonDown("Jump") && !IsGrounded() && isUnlocked && !isGliding)
        {
            isGliding = true;
        }
        else if (Input.GetButtonDown("Jump") && !IsGrounded() && isGliding)
        {
            isGliding = false;
        }
    }
    private void ChargeJumpInput()
    {
        AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.ChargeJumping, out bool isUnlocked);
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.ChargeJumping, out bool isSelected);

        if (Input.GetKeyDown(KeyCode.F) && isUnlocked && isSelected)
        {
            isChargeJumpActivated = !isChargeJumpActivated;
        }

        if (IsGrounded() && Input.GetButton("Jump") && isUnlocked && isChargeJumpActivated && !isChargingJump)
        {
            isChargingJump = true;
        }
    }
    private void SnowDiveInput()
    {
        AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.SnowDiving, out bool isUnlocked);
        if (Input.GetKey(KeyCode.LeftControl) && isUnlocked && IsInSnow())
        {
            isSnowDiving = true;
        }
        else if (isUnlocked && !IsInSnow())
        {
            ClimbSnowWall();
        }
        else if (!isUnlocked || !IsInSnow())
        {
            isSnowDiving = false;
        }
    }
    private void TelegrabInput()
    {
        AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.TeleGrabbing, out bool isUnlocked);
        AbilityCycle.instance.activeAbilities.TryGetValue(Abilities.TeleGrabbing, out bool isSelected);

        if (Input.GetKeyDown(KeyCode.F) && isUnlocked && isSelected)
        {
            isTelegrabActivated = !isTelegrabActivated;
            ActivateTelegrabCamera();
        }
    }
    #endregion
    #endregion
    private void MovePlayer()
    {
        Walk();
        Sprint();
        AbilityMovements();
    }
    private void AbilityMovements()
    {
        Swim();
        SnowDive();
        Glide();
        ChargeJump();
    }
    #region NORMAL MOVEMENT
    private void Walk()
    {
        if (IsGrounded())
        {
            rb.mass = 1f;
            rb.drag = groundDrag;
            isGliding = false;
        }
        else
        {
            rb.drag = 0;
        }

        if (IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
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
            rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
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
        chargeJumpTimer = 14;
        isChargingJump = false;
    }
    #endregion
    #region SWIMMING
    private void Swim()
    {
        if (IsInWater())
        {
            if (!rb.useGravity)
            {
                rb.useGravity = true;
            }

            rb.AddForce(moveDirection.normalized * swimSpeed * 10f, ForceMode.Force);
            playerAnimator.SetFloat("horMove", 1);
            playerAnimator.SetFloat("vertMove", 0);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.speed = 0.7f;

            if (!swimmingAudio.isPlaying)
            {
                swimmingAudio.Play();
            }
        }

        if (IsGrounded() && swimmingAudio.isPlaying)
        {
            swimmingAudio.Stop();
        }
    }
    #endregion
    #region GLIDING
    private void Glide()
    {
        if (!IsGrounded() && isGliding)
        {
            if (rb.useGravity)
            {
                glidingMultiplier = 0.1f;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.useGravity = false;
                rb.velocity = new Vector3(0, -1.5f, 0);

                if (!glidingAudio.isPlaying)
                {
                    glidingAudio.Play();
                }
            }

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * glidingMultiplier, ForceMode.Force);
            if (glidingMultiplier < 0.5)
            {
                glidingMultiplier += 0.005f;
            }

            rb.velocity = new Vector3(rb.velocity.x, -1.5f, rb.velocity.z);
            //gliding animation here

            playerAnimator.SetBool("isGrounded", false);
            playerAnimator.SetBool("isGliding", true);

            AbilityManager.instance.TryActivateAbility(Abilities.Gliding);
        }

        if (!IsGrounded() && !isGliding && !IsInWater())
        {
            DisableGlider();
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //in air animation here
            playerAnimator.SetBool("isGrounded", false);
        }
    }
    private void DisableGlider()
    {
        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }
        playerAnimator.SetBool("isGliding", false);
    }
    #endregion
    #region CHARGEJUMPING
    private void ChargeJump()
    {
        if (isChargingJump)
        {
            rb.velocity = new Vector3(0f, 0f, 0f);

            if (chargeJumpTimer < chargeJumpHeight)
            {
                //audio play
                if (!chargeJumpAudio.isPlaying)
                {
                    chargeJumpAudio.Play();
                }

                chargeJumpTimer = chargeJumpTimer + 0.3f;

                //charging animation here
                playerAnimator.SetBool("isChargingJump", true);
                playerAnimator.SetFloat("horMove", horizontalInput);
                playerAnimator.SetFloat("vertMove", verticalInput);

            }
            AbilityManager.instance.TryActivateAbility(Abilities.ChargeJumping);
        }
    }
    private void ReleaseChargedJump()
    {
        if (chargeJumpTimer != 14 && Input.GetButtonUp("Jump"))
        {
            chargeJumpAudio.Stop();
            rb.AddForce(transform.up * chargeJumpTimer, ForceMode.Impulse);
            isChargingJump = false;
            //chargejump animation here
            playerAnimator.SetBool("isChargingJump", false);
            playerAnimator.SetBool("isJumping", false);
            Invoke(nameof(ResetJump), 0);
        }
    }
    #endregion
    #region SNOWDIVING
    private void SnowDive()
    {
        if (isSnowDiving && IsGrounded())
        {
            playerAnimator.SetBool("isGliding", false);
            rb.AddForce(moveDirection.normalized * snowDiveSpeed * 10f, ForceMode.Force);
            //snow diving animation here
        }
    }
    private void ClimbSnowWall()
    {
        if (HasClimbWallCollision())
        {
            //Debug.Log("wall detected");
            RaycastHit hit;
            if (Physics.Raycast(cameraPosition.position, cameraPosition.forward, out hit, 50f, climbWallLayerMask))
            {
                Debug.DrawLine(cameraPosition.position, hit.point);
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    //climbing animation here (later will make more code for this)
                    gameObject.transform.position = hit.transform.GetChild(0).position;
                }
            }
        }
    }
    private bool HasClimbWallCollision()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.z, climbWallLayerMask, QueryTriggerInteraction.Ignore);
    }
    #endregion
    #region TELEGRABBING
    private void Telegrab()
    {
        if (isTelegrabbing || isObjectGrabbed)
        {
            RaycastHit hitInfo;

            if (isObjectGrabbed && Input.GetMouseButtonDown(0))
            {
                foreach (TelegrabObject t in telegrabObjects)
                {
                    t.gameObject.GetComponent<MeshRenderer>().material = t.telegrabMaterial;
                }

                grabbedGameObject.transform.parent = null;
                grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
                isObjectGrabbed = false;
                telegrabObjects.Clear();
            }
            else if (Physics.Raycast(cameraPosition.position, cameraPosition.forward, out hitInfo, viewDistance, moveableLayerMask) && !isObjectGrabbed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    grabbedGameObject = hitInfo.transform.gameObject;
                    if (grabbedGameObject.transform.childCount != 0)
                    {
                        List<GameObject> list = new List<GameObject>();
                        for (int i = 0; i < grabbedGameObject.transform.childCount; i++)
                        {
                            list.Add(grabbedGameObject.transform.GetChild(i).gameObject);
                            telegrabObjects.Add(grabbedGameObject.transform.GetChild(i).GetComponent<TelegrabObject>());
                        }

                        foreach (GameObject go in list)
                        {
                            go.GetComponent<MeshRenderer>().material = materialForGrabbedObject;
                            grabbedGameObject.GetComponent<MeshRenderer>().material = materialForGrabbedObject;
                        }
                    }
                    else
                    {
                        grabbedGameObject.GetComponent<MeshRenderer>().material = materialForGrabbedObject;
                    }
                    grabbedGameObject.transform.parent = grabbedObjectPosition;
                    grabbedGameObject.transform.position = grabbedObjectPosition.position;
                    grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                    isObjectGrabbed = true;
                    telegrabAudio.Play();
                }
            }
        }
    }
    private void ActivateTelegrabCamera()
    {
        if (!isTelegrabbing)
        {
            isTelegrabbing = true;
            cameraMovement.currentStyle = CameraMovement.CameraStyle.Telegrab;
            cameraMovement.freeLookCam.SetActive(false);
            cameraMovement.telegrabCam.SetActive(true);
            StartCoroutine(CrosshairEnable());
        }
        else
        {
            isTelegrabbing = false;
            cameraMovement.currentStyle = CameraMovement.CameraStyle.Basic;
            cameraMovement.freeLookCam.SetActive(true);
            cameraMovement.telegrabCam.SetActive(false);
            StartCoroutine(CrosshairDisable());
        }
        IEnumerator CrosshairEnable()
        {
            yield return new WaitForSeconds(0.2f);
            telegrabUI.SetActive(true);
        }
        IEnumerator CrosshairDisable()
        {
            yield return new WaitForSeconds(0.2f);
            telegrabUI.SetActive(false);
        }
    }
    #endregion
    #region CHECKS
    private bool IsGrounded()
    {
        RaycastHit hitInfo;

        return Physics.CheckSphere(foxMiddle.position, boxSize.y, groundLayerMask, QueryTriggerInteraction.Ignore) && (Physics.Raycast(foxMiddle.position, -foxMiddle.up, out hitInfo, 1.5f, groundLayerMask) || Physics.Raycast(foxBottom.position, -foxBottom.up, out hitInfo, 1.5f, groundLayerMask));
    }
    private bool IsInWater()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.y, waterLayerMask, QueryTriggerInteraction.Ignore);
    }
    private bool IsInSnow()
    {
        return Physics.CheckSphere(foxMiddle.position, boxSize.y, snowLayerMask, QueryTriggerInteraction.Ignore);
    }
    public bool IsOnSlope()
    {
        return Physics.Raycast(foxMiddle.position, Vector3.down, out hit3, playerHeight * 0.5f + 0.2f) && hit3.normal != Vector3.up;
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
    //public List<float> CollectPlayerPositionForSaving()
    //{
    //    string activeSceneName = SceneManager.GetActiveScene().name;
    //    string overworldSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld);

    //    if (activeSceneName == overworldSceneName)
    //    {
    //        return new List<float> { transform.position.x, transform.position.y, transform.position.z };   
    //    }
    //    else
    //    {
    //        return new List<float> { 1627f, 118f, 360f };
    //    }
    //}

    //private void LoadPlayerPosition()
    //{
    //    transform.position = new Vector3(
    //        SaveManager.instance.GetLoadedPlayerPositionData()[0], 
    //        SaveManager.instance.GetLoadedPlayerPositionData()[1], 
    //        SaveManager.instance.GetLoadedPlayerPositionData()[2]);
    //}
}