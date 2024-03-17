using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    [SerializeField] float swimSpeed = 5f;
    public Transform orientation;
    public float SprintSpeed = 12f;
    bool sprinting;
    public CameraMovement cameraMovement;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Speed Limit")]
    public float groundDrag = 5f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float jumpCooldown = 1f;
    [SerializeField] float airMultiplier = 0.7f;
    [SerializeField] float glidingMultiplier = 0.4f;
    bool readytoJump = true;

    [Header("Checks")]
    public LayerMask GroundLayerMask;
    public Vector3 boxSize;
    [SerializeField] Transform foxMiddle;
    [SerializeField] Transform foxBottom;
    [SerializeField] Transform fox;
    [SerializeField] Transform arcticFox;
    [SerializeField] Transform Camera;
    [SerializeField] private Transform foxHead;
    public LayerMask WaterLayerMask;
    public LayerMask ClimbWallLayerMask;
    public LayerMask SnowLayerMask;
    public LayerMask MoveableLayerMask;
    [SerializeField] float viewDistance;

    [Header("Abilities")]
    [SerializeField] bool glider;
    [SerializeField] bool glidingNow;
    [SerializeField] bool canChargedJump;
    [SerializeField] private float chargeJumpTimer;
    private bool isChargeJumping;
    [SerializeField] private float ChargeJumpHeight = 22f;
    private bool snowDive;
    [SerializeField] float snowDiveSpeed = 15f;
    private GameObject grabbedGameObject;
    [SerializeField] private TelegrabObject TelegrabObject;
    [SerializeField] private bool grabbing;
    [SerializeField] private bool canTeleGrab;
    [SerializeField] private Transform GrabPosition;
    [SerializeField] private Material grabbedMat;
    [SerializeField] private bool TelegrabEnabled;
    [SerializeField] private GameObject TelegrabUI;
    
    List<TelegrabObject> telegrabObjects = new List<TelegrabObject>();
    [SerializeField] AbilityCycle abilityCycle;


    [Header("Animations")]
    public Animator playerAnimator;
    public List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();

    [Header("Audio")]
    [SerializeField] AudioSource ChargeJumpAudio;
    [SerializeField] AudioSource ChargeJumpLandingAudio;
    [SerializeField] AudioSource GlidingAudio;
    [SerializeField] AudioSource FreezingAudio;
    [SerializeField] AudioSource SnowDivingAudio;
    [SerializeField] AudioSource SwimmingAudio;
    [SerializeField] AudioSource TelegrabAudio;

    public static FoxMovement instance;

    // Slopes
    public RaycastHit hit3;
    [SerializeField] private float playerHeight;
    [SerializeField] bool isLoaded;

    private void Awake()
    {
        instance = this;
        //if (SceneManager.GetActiveScene()==SceneManager.GetSceneByBuildIndex(3)||SceneManager.GetSceneByBuildIndex(3).isLoaded) 
        //{
        //    LoadPlayerPosition();
        //}
    }

    void Start()
    {
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetSceneByBuildIndex(3).isLoaded)
        //{
        //    LoadPlayerPosition();
        //}

        rb = GetComponent<Rigidbody>();
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
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3)&&!isLoaded)
        //{
        //    LoadPlayerPosition();

        //    isLoaded = true;
        //}

        if (!DialogueManager.instance.isDialoguePlaying)
        {
            MyInput();
        }

        SpeedControl();

        if (GroundCheck())
        {
            rb.mass = 1f;
            rb.drag = groundDrag;
            glider = false;
        }

        else
        {
            rb.drag = 0;
        }

        OnSlopeCheck();
    }
    private void FixedUpdate()
    {
        if (!DialogueManager.instance.isDialoguePlaying)
        {
            MovePlayer();
        }
    }

    private void MyInput()
    {
        //Movement direction check
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //enable abilities
        if (Input.GetKeyDown(KeyCode.F) && abilityCycle.equippedAbility.officialIndex == 2)
        {
            abilityCycle.equippedAbility.isActivated = !abilityCycle.equippedAbility.isActivated;
            canChargedJump = !canChargedJump;
        }
        if (Input.GetKeyDown(KeyCode.F) && abilityCycle.equippedAbility.officialIndex == 6)
        {
            abilityCycle.equippedAbility.isActivated = !abilityCycle.equippedAbility.isActivated;
            canTeleGrab = !canTeleGrab;
            ActivateTelegrabCamera();
        }
        //Jump check
        if (Input.GetButtonDown("Jump") && readytoJump && GroundCheck() && !canChargedJump)
        {
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (Input.GetButtonDown("Jump") && !GroundCheck() && PlayerManager.instance.hasAbilityValues[0] && !glider)
        {
            glider = true;
        }
        else if (Input.GetButtonDown("Jump") && !GroundCheck() && glider)
        {
            glider = false;
        }
        else if (GroundCheck() && Input.GetButton("Jump") && canChargedJump && !isChargeJumping)
        {
            isChargeJumping = true;

        }

        //Sprint check
        if (Input.GetKey(KeyCode.LeftShift))
            sprinting = true;
        else
            sprinting = false;

        //SnowDive check
        if (Input.GetKey(KeyCode.LeftControl) && PlayerManager.instance.hasAbilityValues[3] && SnowCheck())
            snowDive = true;
        else
            snowDive = false;

        if (chargeJumpTimer != 14 && Input.GetButtonUp("Jump"))
        {
            ChargeJumpAudio.Stop();
            rb.AddForce(transform.up * chargeJumpTimer, ForceMode.Impulse);
            isChargeJumping = false;
            //chargejump animation here
            playerAnimator.SetBool("isChargingJump", false);
            playerAnimator.SetBool("isJumping", false);
            Invoke(nameof(ResetJump), 0);
        }
        if (PlayerManager.instance.hasAbilityValues[3])
        {
            ClimbWall();
        }
        if (TelegrabEnabled || grabbing)
        {
            Telegrab();
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //stop swimming audio
        if (GroundCheck() && SwimmingAudio.isPlaying)
        {
            SwimmingAudio.Stop();
        }
        //in air
        if (!GroundCheck() && !WaterCheck())
        {
            if (glider)
            {
                Glider();
            }
            else if (!glider)
            {
                DisableGlider();
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
                //in air animation here
                playerAnimator.SetBool("isGrounded", false);
            }



        }

        else if (moveDirection == Vector3.zero && GroundCheck())
        {
            //idle animation here

            SetDefaultAnimatorValues();
        }

        //snow diving
        else if (snowDive && GroundCheck())
        {
            playerAnimator.SetBool("isGliding", false);
            rb.AddForce(moveDirection.normalized * snowDiveSpeed * 10f, ForceMode.Force);
            //snow diving animation here
        }

        //sprinting
        else if (sprinting && GroundCheck())
        {
            rb.AddForce(moveDirection.normalized * SprintSpeed * 10f, ForceMode.Force);
            //running animation here

            SetDefaultAnimatorValues();
        }


        //on ground
        else if (GroundCheck())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            //walking animation here
            playerAnimator.speed = 1f;

            SetDefaultAnimatorValues();
        }

        //gliding
        else if (!GroundCheck() && glider)
        {
            Glider();
        }
        else if (!GroundCheck() && !glider && !WaterCheck())
        {
            DisableGlider();
        }
        //swimming
        else if (WaterCheck())
        {
            Swim();
        }

        if (isChargeJumping)
        {
            ChargeJump();
        }


    }
    private void ActivateTelegrabCamera()
    {
        if (!TelegrabEnabled)
        {
            TelegrabEnabled = true;
            cameraMovement.currentStyle = CameraMovement.CameraStyle.Telegrab;
            cameraMovement.freeLookCam.SetActive(false);
            cameraMovement.telegrabCam.SetActive(true);
            StartCoroutine(CrosshairEnable());
        }
        else
        {
            TelegrabEnabled = false;
            cameraMovement.currentStyle = CameraMovement.CameraStyle.Basic;
            cameraMovement.freeLookCam.SetActive(true);
            cameraMovement.telegrabCam.SetActive(false);
            StartCoroutine(CrosshairDisable());
        }
        IEnumerator CrosshairEnable()
        {
            yield return new WaitForSeconds(0.2f);
            TelegrabUI.SetActive(true);
        }
        IEnumerator CrosshairDisable()
        {
            yield return new WaitForSeconds(0.2f);
            TelegrabUI.SetActive(false);
        }
    }

    private void SetDefaultAnimatorValues()
    {
        playerAnimator.SetFloat("horMove", horizontalInput);
        playerAnimator.SetFloat("vertMove", verticalInput);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isGliding", false);
        playerAnimator.SetBool("isGrounded", true);
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
        readytoJump = true;
        chargeJumpTimer = 14;
        isChargeJumping = false;
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

    #region CHECKS
    bool GroundCheck()
    {
        if (Physics.CheckSphere(foxMiddle.position, boxSize.y, GroundLayerMask, QueryTriggerInteraction.Ignore))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(foxMiddle.position, -foxMiddle.up, out hitInfo, 1.5f, GroundLayerMask))
            {

                Debug.DrawLine(foxMiddle.position, hitInfo.point);
                return true;

            }
            else if (Physics.Raycast(foxBottom.position, -foxBottom.up, out hitInfo, 1.5f, GroundLayerMask))
            {

                Debug.DrawLine(foxBottom.position, hitInfo.point);
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool WaterCheck()
    {
        if (Physics.CheckSphere(foxMiddle.position, boxSize.y, WaterLayerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool SnowCheck()
    {
        if (Physics.CheckSphere(foxMiddle.position, boxSize.y, SnowLayerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool OnSlopeCheck()
    {
        if (Physics.Raycast(foxMiddle.position, Vector3.down, out hit3, playerHeight * 0.5f + 0.2f))
        {
            if (hit3.normal != Vector3.up)
            {
                Debug.DrawLine(foxMiddle.position, hit3.point, Color.red);

                return true;
            }
        }
        return false;
    }
    #endregion

    #region CHARGEJUMPING
    //TODO: Move to its own ability script ChargeJumping.cs
    private void ChargeJump()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);

        if (chargeJumpTimer < ChargeJumpHeight)
        {
            //audio play
            if (!ChargeJumpAudio.isPlaying)
            {
                ChargeJumpAudio.Play();
            }

            chargeJumpTimer = chargeJumpTimer + 0.3f;

            //charging animation here'
            playerAnimator.SetBool("isChargingJump", true);
            playerAnimator.SetFloat("horMove", horizontalInput);
            playerAnimator.SetFloat("vertMove", verticalInput);

        }
    }
    #endregion

    #region GLIDING
    //TODO: Move to its own ability script Gliding.cs
    private void Glider()
    {
        if (rb.useGravity)
        {
            glidingMultiplier = 0.1f;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.useGravity = false;
            rb.velocity = new Vector3(0, -1.5f, 0);

            //audio play
            if (!GlidingAudio.isPlaying)
            {
                GlidingAudio.Play();
            }
        }
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * glidingMultiplier, ForceMode.Force);
        if (glidingMultiplier<0.5)
        {
            glidingMultiplier += 0.005f;
        }
        rb.velocity = new Vector3(rb.velocity.x, -1.5f, rb.velocity.z);
        //gliding animation here

        playerAnimator.SetBool("isGrounded", false);
        playerAnimator.SetBool("isGliding", true);
    }

    //TODO: Intergrate in Gliding.cs
    private void DisableGlider()
    {
        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }
        playerAnimator.SetBool("isGliding", false);
    }
    #endregion

    #region TELEGRABBING
    //TODO: Move to its own ability script TeleGrabbing.cs
    private void Telegrab()
    {
        RaycastHit hitInfo;

        //Drop grabbed item
        if (grabbing && Input.GetMouseButtonDown(0))
        {
            grabbedGameObject.transform.gameObject.GetComponent<MeshRenderer>().material = TelegrabObject.telegrabMaterial;
            foreach (TelegrabObject t in telegrabObjects)
            {
                t.gameObject.GetComponent<MeshRenderer>().material = t.telegrabMaterial;
            }
            grabbedGameObject.transform.parent = null;
            grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
            //grabbedGameObject = null;
            grabbing = false;
            //isHighlighted = false;
            telegrabObjects.Clear();
        }
        else if (Physics.Raycast(Camera.position, Camera.forward, out hitInfo, viewDistance, MoveableLayerMask) && !grabbing)
        {


            Debug.DrawLine(Camera.position, hitInfo.point);

            //grab item
            if (!grabbing)
            {
                //isHighlighted = true;
                //hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = grabbableMat;
                if (Input.GetMouseButtonDown(0))
                {
                    grabbedGameObject = hitInfo.transform.gameObject;
                    TelegrabObject = hitInfo.transform.gameObject.GetComponent<TelegrabObject>();
                    if (grabbedGameObject.transform.childCount != 0)
                    {
                        List<GameObject> list = new List<GameObject>();
                        for (int i = 0; i < grabbedGameObject.transform.childCount; i++)
                        {
                            list.Add(grabbedGameObject.transform.GetChild(i).gameObject);
                            telegrabObjects.Add(grabbedGameObject.transform.GetChild(i).GetComponent<TelegrabObject>());
                        }

                        foreach (GameObject go in list)
                            go.GetComponent<MeshRenderer>().material = grabbedMat;
                        grabbedGameObject.GetComponent<MeshRenderer>().material = grabbedMat;
                    }
                    else
                    {
                        grabbedGameObject.GetComponent<MeshRenderer>().material = grabbedMat;
                    }
                    grabbedGameObject.transform.parent = GrabPosition;
                    grabbedGameObject.transform.position = GrabPosition.position;
                    grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                    //grabbedGameObject.transform.rotation = Quaternion.identity;
                    grabbing = true;
                    TelegrabAudio.Play();
                }

            }
        }
    }
    #endregion

    #region WALLCLIMBING
    //TODO: Move to its own ability script WallClimbing.cs
    private void ClimbWall()
    {
        if (ClimbWallCheck())
        {
            Debug.Log("wall detected");
            RaycastHit hitInfo2;
            if (Physics.Raycast(Camera.position, Camera.forward, out hitInfo2, 50f, ClimbWallLayerMask))
            {

                Debug.DrawLine(Camera.position, hitInfo2.point);
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    //climbing animation here (later will make more code for this)
                    gameObject.transform.position = hitInfo2.transform.GetChild(0).position;

                }

            }
        }
    }

    //NOTE: Intergrate in WallClimbing.cs?
    bool ClimbWallCheck()
    {
        if (Physics.CheckSphere(foxMiddle.position, boxSize.z, ClimbWallLayerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region SWIMMING
    //TODO: Move to its own ability script Swimming.cs
    private void Swim()
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
        if (!SwimmingAudio.isPlaying)
        {
            SwimmingAudio.Play();
        }
    }
    #endregion



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foxMiddle.position, boxSize.y);
    }

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
