using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class FoxMovement : MonoBehaviour
{
    public static FoxMovement Instance;

    [FormerlySerializedAs("cameraMovement")] [Header("Movement")]
    public CameraMovement CameraMovement;
    [FormerlySerializedAs("rb")] public Rigidbody Rb;
    [FormerlySerializedAs("moveSpeed")] public float MoveSpeed = 7f;
    [FormerlySerializedAs("sprintSpeed")] public float SprintSpeed = 12f;
    private bool _canMove;
    [FormerlySerializedAs("enableMovementOnStart")] [SerializeField] private bool _enableMovementOnStart;

    [FormerlySerializedAs("orientation")] [SerializeField] private Transform _orientation;
    [FormerlySerializedAs("groundDrag")] [SerializeField] private float _groundDrag = 5f;

    [FormerlySerializedAs("isSprinting")] public bool IsSprinting;
    [FormerlySerializedAs("horizontalInput")] [HideInInspector] public float HorizontalInput;
    [FormerlySerializedAs("verticalInput")] [HideInInspector] public float VerticalInput;
    private Vector3 _moveDirection;

    [Header("Slopes")]
    public RaycastHit SlopeHit;
    [FormerlySerializedAs("playerHeight")] [SerializeField] private float _playerHeight;
    [FormerlySerializedAs("maxSlopeAngle")] [SerializeField] private float _maxSlopeAngle;
    private bool _exitingSlope;

    [FormerlySerializedAs("jumpForce")]
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 15f;
    [FormerlySerializedAs("jumpCooldown")] [SerializeField] private float _jumpCooldown = 1f;

    [FormerlySerializedAs("isReadyToJump")] [HideInInspector] public bool IsReadyToJump = true;
    [FormerlySerializedAs("isReadyToSwim")] [HideInInspector] public bool IsReadyToSwim = true;
    [FormerlySerializedAs("isReadyToShake")] [HideInInspector] public bool IsReadyToShake = true;



    [FormerlySerializedAs("groundLayerMask")]
    [Header("Checks")]
    [SerializeField] private LayerMask _groundLayerMask;
    [FormerlySerializedAs("waterLayerMask")] [SerializeField] private LayerMask _waterLayerMask;
    [FormerlySerializedAs("climbWallLayerMask")] public LayerMask ClimbWallLayerMask;
    [FormerlySerializedAs("snowLayerMask")] [SerializeField] private LayerMask _snowLayerMask;
    [FormerlySerializedAs("moveableLayerMask")] public LayerMask MoveableLayerMask;
    [FormerlySerializedAs("cameraPosition")] public Transform CameraPosition;
    [FormerlySerializedAs("lookAtTarget")] [SerializeField] private Transform _lookAtTarget;
    [FormerlySerializedAs("foxMiddle")] [SerializeField] public Transform FoxMiddle;
    [FormerlySerializedAs("foxFront")] [SerializeField] public Transform FoxFront;
    [FormerlySerializedAs("foxBottom")] [SerializeField] private Transform _foxBottom;
    //[SerializeField] private Transform fox;
    //[SerializeField] private Transform arcticFox;

    //private float viewDistance = 50f;
    [FormerlySerializedAs("boxSize")] [SerializeField] private Vector3 _boxSize = new Vector3(0f, 2f, 2f);

    private AbilityCycle _abilityCycle;
    private bool _isLoaded;
    private bool _wasGrounded;
    private bool _grounded;
    private float _jumpApex;
    private ParticleSystem[] _landingEffects;

    [FormerlySerializedAs("playerAnimator")] [Header("Animations")]
    public Animator PlayerAnimator;
    private List<AnimatorControllerParameter> _animatorBools = new List<AnimatorControllerParameter>();
    [FormerlySerializedAs("cinematicCamAnimator")] public Animator CinematicCamAnimator;

    [FormerlySerializedAs("telegrabEffect")] [Header("VFX")]
    public VisualEffect TelegrabEffect;

    [FormerlySerializedAs("redFox")]
    [Header("Fox Models")]
    [SerializeField] private GameObject _redFox;
    [FormerlySerializedAs("arcticFox")] [SerializeField] private GameObject _arcticFox;

    private void Awake()
    {
        if (FoxMovement.Instance != null)
        {
            Debug.LogWarning("There is more than one FoxMovement in the scene!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
        _landingEffects = new ParticleSystem[4];
        foreach (Transform t in transform)
        {
            if (t.gameObject.name == "Landing_WaterImpact")
                _landingEffects[0] = t.GetComponent<ParticleSystem>();
            else if (t.gameObject.name == "Landing_WaterImpactDrops")
                _landingEffects[1] = t.GetComponent<ParticleSystem>();
            else if (t.gameObject.name == "Landing_SnowImpact_PS")
                _landingEffects[2] = t.GetComponent<ParticleSystem>();
            else if (t.gameObject.name == "Landing_DustImpact_PS")
                _landingEffects[3] = t.GetComponent<ParticleSystem>();
        }
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("OverWorld"))
        {
            _canMove = true;

            if (File.Exists(SaveManager.Instance.SaveFilePath))
            {
                Invoke(nameof(LoadPlayerPosition), 0.2f);
                //LoadPlayerPosition();
            }
        }

        if (_enableMovementOnStart)
        {
            _canMove = true;
        }

        Rb.freezeRotation = true;
        _abilityCycle = GetComponent<AbilityCycle>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        if (SceneManager.GetActiveScene().name.Contains("Overworld"))
        {
            CinematicCamAnimator = GameObject.Find("IntroCamera").GetComponent<Animator>();
        }

        foreach (AnimatorControllerParameter item in PlayerAnimator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                _animatorBools.Add(item);
            }
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.DialogueEvents.OnStartDialogue += DisableMovementForDialogue;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += EnableMovementAfterDialogue;
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleMovementBasedOnInputActions;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.DialogueEvents.OnStartDialogue -= DisableMovementForDialogue;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue -= EnableMovementAfterDialogue;
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleMovementBasedOnInputActions;
    }

    void Update()
    {
        _grounded = IsGrounded();
        Landing();
        SpeedControl();
        IsOnSlope();
        Animations();

        if (!DialogueManager.Instance.IsDialoguePlaying && _canMove)
        {
            ProcessInput();
        }
        _wasGrounded = _grounded;
    }
    private void FixedUpdate()
    {
        if (!DialogueManager.Instance.IsDialoguePlaying && _canMove)
        {
            MovePlayer();
        }
    }
    #region INPUTS
    private void ProcessInput()
    {
        HorizontalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().x;
        VerticalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().y;
        _moveDirection = _orientation.forward * VerticalInput + _orientation.right * HorizontalInput;

        SprintInput();
        JumpInput();
        AbilityInputs();
        SelectAbility();
        SitAndLay();
    }

    void AbilityInputs()
    {
        //gliding
        if (PlayerInputHandler.Instance.JumpInput.WasPressedThisFrame() && !_grounded && !IsInWater())
        {
            AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.Gliding);
        }

        //swimming
        AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.Swimming);

        //chargejumping
        if (PlayerInputHandler.Instance.JumpInput.WasPressedThisFrame() && _grounded)
        {
            AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.ChargeJumping);
        }

        //snowdiving
        if (PlayerInputHandler.Instance.SnowDiveInput.IsPressed() && IsInSnow())
        {
            AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.SnowDiving);
        }

        //telegrabbing
        if (PlayerInputHandler.Instance.TelegrabGrabInput.WasPressedThisFrame())
        {
            AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.TeleGrabbing);
        }
    }
    void SelectAbility()
    {
        //chargejump
        AbilityCycle.Instance.ActiveAbilities.TryGetValue(Abilities.ChargeJumping, out bool isChargeJumpSelected);
        if (PlayerInputHandler.Instance.UseAbilityInput.WasPressedThisFrame() && isChargeJumpSelected)
        {
            ChargeJumping.Instance.IsChargeJumpActivated = !ChargeJumping.Instance.IsChargeJumpActivated;
        }
        if (!isChargeJumpSelected)
        {
            ChargeJumping.Instance.IsChargeJumpActivated = false;
        }

        //telegrab
        AbilityCycle.Instance.ActiveAbilities.TryGetValue(Abilities.TeleGrabbing, out bool isTelegrabSelected);
        if (PlayerInputHandler.Instance.UseAbilityInput.WasPressedThisFrame() && isTelegrabSelected)
        {
            TeleGrabbing.Instance.ActivateTelegrabCamera();
            TeleGrabbing.Instance.IsTelegrabActivated = !TeleGrabbing.Instance.IsTelegrabActivated;
        }
        if (!isTelegrabSelected && TeleGrabbing.Instance.IsTelegrabActivated)
        {
            TeleGrabbing.Instance.IsTelegrabActivated = false;
            TeleGrabbing.Instance.ActivateTelegrabCamera();
        }
    }
    private void Landing()
    {
        if (!_wasGrounded && _grounded)
        {
            float scale = (_jumpApex - transform.position.y) * .25f;
            if (scale > 1.5f) scale = 1.5f;
            else if (scale < .05f) scale = .05f;
            if (IsInWater())
            {
                _landingEffects[0].transform.localScale = new Vector3(scale, scale, scale);
                _landingEffects[0].Play();
                _landingEffects[1].transform.localScale = new Vector3(scale, scale, scale);
                _landingEffects[1].Play();
            }

            else
            {
                if (_redFox.activeInHierarchy)
                {
                    _landingEffects[3].transform.localScale = new Vector3(scale, scale, scale);
                    _landingEffects[3].Play();
                }

                else if (_arcticFox.activeInHierarchy)
                {
                    _landingEffects[2].transform.localScale = new Vector3(scale, scale, scale);
                    _landingEffects[2].Play();
                }
            }
            _jumpApex = 0;
        }
        else if (!_grounded && transform.position.y > _jumpApex)
            _jumpApex = transform.position.y;

    }
    private void SprintInput()
    {

        IsSprinting = PlayerInputHandler.Instance.SprintInput.IsPressed();
    }
    private void JumpInput()
    {
        if (PlayerInputHandler.Instance.JumpInput.WasPressedThisFrame() && IsReadyToJump && IsGrounded() && !ChargeJumping.Instance.IsChargeJumpActivated && PlayerInputHandler.Instance.JumpInput.enabled)
        {
            IsReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }
    #endregion
    private void Animations()
    {
        Walk();
        Sprint();
        AnimationConditions();
    }
    #region MOVEMENT
    private void MovePlayer()
    {
        float speed = 0f;
        float modifier = 1f;
        AbilityManager.Instance.AbilityStatuses.TryGetValue(Abilities.SnowDiving, out bool isSnowDiveUnlocked);

        SetMovementSpeed(ref speed, ref modifier, isSnowDiveUnlocked);

        //On slope
        if (IsOnSlope() && !_exitingSlope)
        {
            Rb.AddForce(GetSlopeMoveDirection() * MoveSpeed * 20f, ForceMode.Force);

            if (Rb.velocity.y > 0)
            {
                Rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            //turn gravity off when on slope
            Rb.useGravity = !IsOnSlope();
        }

        Rb.AddForce(_moveDirection.normalized * speed * 10f * modifier, ForceMode.Force);

        PlayerAnimator.SetFloat("upMove", Rb.velocity.y);
    }

    private void SetMovementSpeed(ref float speed, ref float modifier, bool isSnowDiveUnlocked)
    {

        //Limit speed on Slope
        if (IsOnSlope() && !_exitingSlope)
        {
            if (Rb.velocity.magnitude > MoveSpeed && !IsSprinting)
            {
                Rb.velocity = Rb.velocity.normalized * MoveSpeed;
            }
            else if (Rb.velocity.magnitude > SprintSpeed && IsSprinting)
            {
                Rb.velocity = Rb.velocity.normalized * SprintSpeed;
            }
        }

        //Walking
        if (_grounded && !IsSprinting)
        {
            Rb.useGravity = true;
            speed = MoveSpeed;

            SetDefaultAnimatorValues();
        }

        //Sprinting
        if (_grounded && IsSprinting)
        {
            speed = SprintSpeed;
            Rb.useGravity = true;
        }

        //Swimming
        if (IsInWater())
        {
            speed = Swimming.Instance.SwimSpeed;
            Rb.useGravity = true;
            PlayerAnimator.SetBool("isSwimming", true);
        }

        //In air, Gliding
        if (!_grounded && !IsInWater() && Gliding.Instance.IsGliding)
        {
            //rb.useGravity = true;
            speed = MoveSpeed;
            modifier = Gliding.Instance.GlidingMultiplier;
        }

        //In air, NOT Gliding
        if (!_grounded && !IsInWater() && !Gliding.Instance.IsGliding)
        {
            //rb.useGravity = true;
            speed = MoveSpeed;
            modifier = Gliding.Instance.AirMultiplier;
        }

        //Walking on snow
        if (IsInSnow() && _grounded && isSnowDiveUnlocked)
        {
            speed = SnowDiving.Instance.SnowDiveSpeed;
        }


    }

    private void Walk()
    {
        if (_grounded)
        {
            Rb.mass = 1f;
            Rb.drag = _groundDrag;

        }
        else
        {
            Rb.drag = 0;
        }

        if (_grounded)
        {
            //walking animation here
            PlayerAnimator.speed = 1f;

            SetDefaultAnimatorValues();
        }

        if (_grounded && _moveDirection == Vector3.zero)
        {
            //idle animation here
            SetDefaultAnimatorValues();
        }
    }

    private void Sprint()
    {
        if (_grounded && IsSprinting)
        {
            if (HorizontalInput != 0 || VerticalInput != 0)
            {
                PlayerAnimator.SetBool("isSprinting", true);
            }

        }
    }
    private void Jump()
    {
        _exitingSlope = true;

        Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);

        Rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        //jumping animation here

        PlayerAnimator.SetBool("isChargingJump", false);
        PlayerAnimator.SetBool("isJumping", true);
        PlayerAnimator.SetBool("isGrounded", false);
        PlayerAnimator.SetBool("isSprinting", false);
    }
    private void ResetJump()
    {
        PlayerAnimator.SetBool("isJumping", false);
        IsReadyToJump = true;
        _exitingSlope = false;
    }
    #endregion
    #region CHECKS
    public bool IsGrounded()
    {
        RaycastHit hitInfo;

        return Physics.CheckSphere(FoxMiddle.position, _boxSize.y, _groundLayerMask, QueryTriggerInteraction.Ignore) && (Physics.Raycast(FoxMiddle.position, -FoxMiddle.up, out hitInfo, 1.5f, _groundLayerMask) || Physics.Raycast(_foxBottom.position, -_foxBottom.up, out hitInfo, 1.5f, _groundLayerMask));
    }
    public bool IsInWater()
    {
        return Physics.CheckSphere(FoxMiddle.position, _boxSize.y * 0.9f, _waterLayerMask, QueryTriggerInteraction.Ignore);
    }
    public bool IsInSnow()
    {
        return Physics.CheckSphere(FoxMiddle.position, _boxSize.y, _snowLayerMask, QueryTriggerInteraction.Ignore);
    }
    public bool IsOnSlope()
    {

        //return Physics.Raycast(foxMiddle.position, Vector3.down, out SlopeHit, playerHeight + 0.2f) && SlopeHit.normal != Vector3.up;

        if (Physics.Raycast(FoxFront.position, Vector3.down, out SlopeHit, _playerHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            Debug.DrawRay(FoxMiddle.position, Vector3.down, Color.cyan);
            return angle < _maxSlopeAngle && angle != 0;

        }
        return false;

    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, SlopeHit.normal).normalized;
    }
    public bool HasClimbWallCollision()
    {
        return Physics.CheckSphere(FoxMiddle.position, _boxSize.z, ClimbWallLayerMask, QueryTriggerInteraction.Ignore);
    }
    #endregion
    #region MISC
    public void SetDefaultAnimatorValues()
    {
        PlayerAnimator.SetFloat("horMove", HorizontalInput, 0.1f, Time.deltaTime);
        PlayerAnimator.SetFloat("vertMove", VerticalInput, 0.1f, Time.deltaTime);
        //playerAnimator.SetBool("isJumping", false);
        PlayerAnimator.SetBool("isGliding", false);
        PlayerAnimator.SetBool("isGrounded", true);
        PlayerAnimator.SetBool("isSprinting", false);
        PlayerAnimator.SetBool("isSwimming", false);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);

        if (flatVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            Rb.velocity = new Vector3(limitedVel.x, Rb.velocity.y, limitedVel.z);
        }
    }
    private void SitAndLay()
    {
        if (PlayerInputHandler.Instance.SitInput.WasPerformedThisFrame())
        {
            PlayerAnimator.SetBool("isSitting", !PlayerAnimator.GetBool("isSitting"));
            PlayerAnimator.SetBool("isLaying", false);
        }

        if (PlayerInputHandler.Instance.LayInput.WasPerformedThisFrame())
        {
            PlayerAnimator.SetBool("isLaying", !PlayerAnimator.GetBool("isLaying"));
            PlayerAnimator.SetBool("isSitting", false);
        }
    }
    private void AnimationConditions()
    {
        if ((PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|FoxLieDownAni")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_StandUp_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_Sitting_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_OutOfWater_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_PickUpFromBush_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_PickUpFromGround_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_EnterWater_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_Playful2_ANI")) || (PlayerInputHandler.Instance.MoveInput.enabled && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PL_FreezingAbility_ANI")))
        {
            PlayerInputHandler.Instance.MoveInput.Disable();
            PlayerInputHandler.Instance.JumpInput.Disable();
        }
        else
        {
            if (!PlayerInputHandler.Instance.MoveInput.enabled)
            {
                if (SceneManager.GetActiveScene().name.Contains("Overworld"))
                {
                    if (!CinematicCamAnimator.enabled)
                    {
                        PlayerInputHandler.Instance.MoveInput.Enable();
                        PlayerInputHandler.Instance.JumpInput.Enable();
                    }

                }
                else
                {
                    PlayerInputHandler.Instance.MoveInput.Enable();
                    PlayerInputHandler.Instance.JumpInput.Enable();
                }

            }

        }
    }

    public void CooldownTrigger(string boolName)
    {
        PlayerAnimator.SetBool(boolName, false);
        if (boolName == "isReadyToSwim")
        {
            IsReadyToSwim = false;
        }
        else
        {
            IsReadyToShake = false;
        }
        StartCoroutine(StartCooldown(boolName));
    }
    IEnumerator StartCooldown(string boolName)
    {

        yield return new WaitForSeconds(30f);
        PlayerAnimator.SetBool(boolName, true);
        if (boolName == "isReadyToSwim")
        {
            IsReadyToSwim = true;
            PlayerAnimator.SetBool(boolName, true);
        }
        else
        {
            IsReadyToShake = true;
            PlayerAnimator.SetBool(boolName, true);
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(FoxMiddle.position, _boxSize.y);
    }
    #endregion

    private void LoadPlayerPosition()
    {
        PositionData loadedData = SaveManager.Instance.GetLoadedPlayerPosition();

        if (loadedData != null)
        {
            Vector3 pos = new Vector3(loadedData.X, loadedData.Y, loadedData.Z);
            Quaternion rot = new Quaternion(loadedData.RotX, loadedData.RotY, loadedData.RotZ, loadedData.RotW);
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
        PositionData data = new PositionData(transform.position, _orientation.transform.rotation);
        Debug.Log($"FM CollectPos Position is: {data.X}, {data.Y}, {data.Z}. Rotation is: {data.RotX}, {data.RotY}, {data.RotZ}");

        return data;
    }

    // prevent jumping when dialogue starts
    private void DisableMovementForDialogue()
    {
        _canMove = false;
        IsReadyToJump = false;
    }

    // get ready to enable jumping when dialogue has ended
    private void EnableMovementAfterDialogue()
    {
        _canMove = true;
        Invoke(nameof(ResetJump), 0.3f);
    }

    private void ToggleMovementBasedOnInputActions(bool movementEnabled)
    {
        _canMove = movementEnabled;
    }
}