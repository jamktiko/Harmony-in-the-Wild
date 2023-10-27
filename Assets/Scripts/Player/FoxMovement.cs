using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Ground Check")]
    public LayerMask GroundLayerMask;
    public Vector3 boxSize;
    [SerializeField] Transform foxMiddle;
    public LayerMask WaterLayerMask;
    public LayerMask ClimbWallLayerMask;
    public LayerMask SnowLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        if (GroundCheck())
            rb.drag = groundDrag;
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

        //Jump check
        if (Input.GetButtonDown("Jump")&&readytoJump&&GroundCheck())
        {
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }

        //Sprint check
        if (Input.GetKey(KeyCode.LeftShift))
            sprinting=true;
        else
            sprinting = false;
    }
    private void MovePlayer() 
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sprinting
        if (sprinting && GroundCheck())
            rb.AddForce(moveDirection.normalized * SprintSpeed * 10f, ForceMode.Force);

        //on ground
        else if (GroundCheck())
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f,ForceMode.Force);

        //in air
        else if (!GroundCheck())
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f*airMultiplier, ForceMode.Force);

        

    }
    private void Jump() 
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readytoJump=true;
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
            return true;
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
