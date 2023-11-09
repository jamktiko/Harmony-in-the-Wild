using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FoxMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed=7f;
    public Transform orientation;
    public float SprintSpeed = 12f;
    bool sprinting;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Speed Limit")]
    public float groundDrag=5f;

    [Header("Jump")]
    [SerializeField]float jumpForce=15f;
    [SerializeField] float jumpCooldown=1f;
    [SerializeField] float airMultiplier=0.7f;
    [SerializeField] float glidingMultiplier = 0.4f;
    bool readytoJump=true;

    [Header("Checks")]
    public LayerMask GroundLayerMask;
    public Vector3 boxSize;
    [SerializeField] Transform foxMiddle;
    [SerializeField] Transform foxBottom;
    [SerializeField] Transform Camera;
    [SerializeField] private Transform foxHead;
    public LayerMask WaterLayerMask;
    public LayerMask ClimbWallLayerMask;
    public LayerMask SnowLayerMask;
    [SerializeField] float viewDistance;

    [Header("Abilities")]
    [SerializeField] bool glider;
    [SerializeField] bool glidingNow;
    [SerializeField] bool canChargedJump;
    private float chargeJumpTimer;
    private bool snowDive;
    [SerializeField] float snowDiveSpeed=15f;

    [Header("Animations")]
    public Animator playerAnimator;
    public List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();

    [Header("Audio")]
    [SerializeField] AudioSource ChargeJumpAudio;
    [SerializeField] AudioSource ChargeJumpLandingAudio;
    [SerializeField] AudioSource GlidingAudio;
    [SerializeField] AudioSource FreezingAudio;
    [SerializeField] AudioSource SnowDivingAudio;


    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerAnimator = GetComponentInChildren<Animator>();
        foreach (AnimatorControllerParameter item in playerAnimator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                animatorBools.Add(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        if (GroundCheck())
        {
            rb.mass = 1f;
            rb.drag = groundDrag;
            glider = false;
        }            
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput() 
    {
        //Movement direction check
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.H))
        {
            
            canChargedJump = !canChargedJump;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerManager.instance.abilityValues[3] = true;
            PlayerManager.instance.abilityValues[0] = !PlayerManager.instance.abilityValues[0];
        }
        //Jump check
        if (Input.GetButtonDown("Jump")&&readytoJump&&GroundCheck()&&!canChargedJump)
        {
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
        else if (Input.GetButtonDown("Jump") && !GroundCheck() && PlayerManager.instance.abilityValues[0]&&!glider)
        {
            glider = true;
        }
        else if (Input.GetButtonDown("Jump") && !GroundCheck()&&glider)
        {
            glider = false;
        }
        else if (GroundCheck() && Input.GetButton("Jump") && canChargedJump)
        {
            ChargeJump();
        }

        //Sprint check
        if (Input.GetKey(KeyCode.LeftShift))
            sprinting=true;
        else
            sprinting = false;

        //SnowDive check
        if (Input.GetKey(KeyCode.LeftControl) && PlayerManager.instance.abilityValues[3]&&SnowCheck())
            snowDive = true; 
        else
            snowDive = false;
        
        if (chargeJumpTimer!=0&&Input.GetButtonUp("Jump"))
        {
            ChargeJumpAudio.Stop();
            rb.AddForce(transform.up * chargeJumpTimer, ForceMode.Impulse);
            //chargejump animation here
            Invoke(nameof(ResetJump), 0);
        }
        if (PlayerManager.instance.abilityValues[3])
        {
            ClimbWall();
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (moveDirection==Vector3.zero&&GroundCheck())
        {
            //idle animation here

            playerAnimator.SetFloat("horMove", horizontalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetFloat("vertMove", verticalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetFloat("vertMove", verticalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
                playerAnimator.SetBool("isGrounded", true);
        }
        //snow diving
        else if (snowDive && GroundCheck())
        {
            rb.AddForce(moveDirection.normalized * snowDiveSpeed * 10f, ForceMode.Force);
            //snow diving animation here
        }
        //sprinting
        else if (sprinting && GroundCheck())
        {
            rb.AddForce(moveDirection.normalized * SprintSpeed * 10f, ForceMode.Force);
            //running animation here

            playerAnimator.SetFloat("horMove", horizontalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetFloat("vertMove", verticalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetBool("isGrounded", true);
            Debug.Log(horizontalInput);
        }


        //on ground
        else if (GroundCheck())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            //walking animation here

            playerAnimator.SetFloat("horMove", horizontalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetFloat("vertMove", verticalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetBool("isGrounded", true);
            Debug.Log(horizontalInput);
        }


        //gliding
        else if (!GroundCheck() && glider)
        {
            Glider();
        }
        else if (!GroundCheck() && !glider)
        {
            DisableGlider();
        }

        //in air
        else if (!GroundCheck()) 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //in air animation here

            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }

            
        }


    }

    private void Glider()
    {
        if (rb.useGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.useGravity = false;
            rb.velocity = new Vector3(0, -1.5f, 0);

            //audio play
            if (!GlidingAudio.isPlaying)
            {
                GlidingAudio.Play();
            }
        }
        rb.AddForce(moveDirection.normalized * moveSpeed *10f*glidingMultiplier, ForceMode.Force);
        rb.velocity = new Vector3(rb.velocity.x, -1.5f, rb.velocity.z);
        //gliding animation here

        foreach (AnimatorControllerParameter item in animatorBools)
        {
            playerAnimator.SetBool(item.name, false);
        }
        playerAnimator.SetBool("isGliding", true);
    }
    private void DisableGlider() 
    {
        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * glidingMultiplier, ForceMode.Force);
    }

    private void Jump() 
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumping animation here

        foreach (AnimatorControllerParameter item in animatorBools)
        {
            playerAnimator.SetBool(item.name, false);
        }
        playerAnimator.SetBool("isGrounded", true);
        playerAnimator.SetBool("isJumping", true);
        
    }

    private void ChargeJump() 
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (chargeJumpTimer < 15f)
        {
            //audio play
            if (!ChargeJumpAudio.isPlaying)
            {
                ChargeJumpAudio.Play();
            }

            chargeJumpTimer = chargeJumpTimer + 0.4f;

            //charging animation here
            playerAnimator.SetFloat("horMove", 0);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetFloat("vertMove", verticalInput);
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                playerAnimator.SetBool(item.name, false);
            }
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetBool("isJumping", true);
        }
    }
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
    private void ResetJump()
    {
        readytoJump=true;
        chargeJumpTimer=0;
    }
    private void SpeedControl() 
    {
        Vector3 flatVel=new Vector3(rb.velocity.x,0f,rb.velocity.z);

        if (flatVel.magnitude>moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity=new Vector3(limitedVel.x,rb.velocity.y, limitedVel.z);
        }
    }
    bool GroundCheck()
    {
        if (Physics.CheckSphere(foxMiddle.position, boxSize.y, GroundLayerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("wall detected");
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foxMiddle.position, boxSize.y);
    }
}
