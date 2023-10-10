using HeneGames.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FoxMove : MonoBehaviour
{
    [SerializeField]CharacterController Controller;
    public float Speed;
    public Transform Cam;
    public float gravity = 15.0F;
    [SerializeField] public Animator animator;
    public List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] CameraMove cameraMove;
    [SerializeField]Transform fox;
    [SerializeField] Transform LookAt;
    float rotation;
    public Vector3 boxSize;
    public float maxDistance;
    [SerializeField] float viewDistance;
    public LayerMask GroundLayerMask;
    public LayerMask WaterLayerMask;
    [SerializeField]LayerMask Moveable;
    public float jumpforce = 10f;
    public float timer = 0f;
    [SerializeField] bool enableGravity=true;
    [SerializeField] float jumpSpeed = 10f;
    Vector3 Jump = new Vector3(0, 0, 0);
    bool sprinting;
    public bool canSwim = false;
    public bool canGlide = false;
    public bool test = false;
    public bool glider = false;
    public bool hasChargedJump = false;
    public bool canChargedJump=false;
    [SerializeField] private float chargeJumpTimer;
    [SerializeField]private float GlidingSpeed;
    [SerializeField] Material grabbableMat;
    [SerializeField] Material grabbedMat;
    [SerializeField] Material OriginialMat;
    public bool canTeleGrab;
    private bool grabbed;
    [SerializeField]private GameObject grabbedGameObject;

    public Transform GrabPosition;
    [SerializeField]private int grabTimer;
    [SerializeField]private TelegrabObject TelegrabObject;
    private bool isHighlighted;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraMove = GetComponentInChildren<CameraMove>();
        foreach (AnimatorControllerParameter item in animator.parameters)
        {
            if (item.type==AnimatorControllerParameterType.Bool)
            {
                animatorBools.Add(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        y= Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        x= Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        if (hasChargedJump&&Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("lol");
            canChargedJump=!canChargedJump;
        }
        if (GroundCheck())
        {
            glider = false;
            test = true;
            //next 2 lines to remove
            animator.SetBool("Jump", false);
            animator.SetBool("Idle", true);

            //idle animation here

            if (Input.GetKey(KeyCode.LeftShift)&&!sprinting)
            {
                sprinting = true;

                //running animation here

                animator.speed = 1.4f;
                Speed = 5;
            }
            else
            {
                Speed = 4;

                //walking animation here

                animator.speed = 1f;
                sprinting = false;
            }
            if (!WaterCheck())
            {
                animator.speed = 1;
            }
            if (Vertical==0&&Horizontal==0)
            {
                //foreach (AnimatorControllerParameter item in animatorBools)
                //{
                //    animator.SetBool(item.name, false);
                //}
                
                //idle animation here

                timer++;
                if (timer > 400 && animator.GetBool("CanSit") != true)
                {
                    //sitting down animation here
                }
            }
            else if (Vertical > 0 || Horizontal > 0 || Horizontal < 0)
            {
                if (cameraMove.X == 0)
                {
                    timer = 0;
                    //running animation here
                }

                else if (cameraMove.X > 0.05)
                {
                    timer = 0;
                    //turning right animation here
                }
                else if (cameraMove.X < -0.05)
                {
                    timer = 0;
                    //turning left animation here
                }
            }
            else if (Vertical < 0)
            {
                timer = 0;
                //walking back animation here
            }
        }
        else if (WaterCheck())
        {
            //foreach (AnimatorControllerParameter item in animatorBools)
            //{
            //    animator.SetBool(item.name, false);
            //}

            //swimming animation here


        }
        else if (!GroundCheck())
        {
            test=false;
            if (Input.GetButtonDown("Jump")&&canGlide)
            {
                glider = true;
            }

            //foreach (AnimatorControllerParameter item in animatorBools)
            //{
            //    animator.SetBool(item.name, false);
            //}

            //jumping animation here

        }
        
        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Vector3 MovementJump = new Vector3(0, 0, 0);
        //Movement.Normalize();
        if (GroundCheck()&&Input.GetButtonDown("Jump")&&!canChargedJump)
        {
            timer = 0;
            MovementJump.y = jumpforce;
            Jump = MovementJump;
            Debug.Log("hi");

            enableGravity = false;
            StartCoroutine(disableGravity());

            //FOR TESTING PURPORSES ONLY, REMOVE LATER WHEN NO LONGER NEEDED!!
            if (QuestManager.instance.CheckQuestState("TestJumpQuest").Equals(QuestState.IN_PROGRESS))
            {
                FindObjectOfType<TestJumpQuestStep>().JumpProgress();
            }
        }
        else if (GroundCheck() && Input.GetButton("Jump") && canChargedJump)
        {
            if (chargeJumpTimer<5f)
            {
                chargeJumpTimer = chargeJumpTimer + 0.05f;
            }
            

            timer = 0;

            // FOR TESTING PURPORSES ONLY, REMOVE LATER WHEN NO LONGER NEEDED!!
            if (QuestManager.instance.CheckQuestState("TestJumpQuest").Equals(QuestState.IN_PROGRESS))
            {
                FindObjectOfType<TestJumpQuestStep>().JumpProgress();
            }
        }
        else if (enableGravity)
        {
            if (glider) 
            {
                Jump.y -= GlidingSpeed * Time.deltaTime;
            }
            else
            {
                Jump.y -= gravity * Time.deltaTime;
            }
        }
        
        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);
            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        }
        if (chargeJumpTimer!=0&&Input.GetButtonUp("Jump"))
        {
            MovementJump.y = chargeJumpTimer;
            Jump = MovementJump;
            enableGravity = false;
            StartCoroutine(disableGravity());
            chargeJumpTimer = 0;
        }
        if (!canGlide)
        {
            canGlide = PlayerManager.instance.abilityValues[0];
        }
        if (!hasChargedJump) 
        {
            hasChargedJump = PlayerManager.instance.abilityValues[2];
        }
        if (!canTeleGrab) 
        {
            canTeleGrab = PlayerManager.instance.abilityValues[6];
        }
        Controller.Move(Jump * Time.deltaTime * jumpSpeed);

        Controller.Move(Movement * Speed);
        //Telegrab ability
        if (grabbed && Input.GetKeyDown(KeyCode.B))
        {

            grabbedGameObject.transform.gameObject.GetComponent<MeshRenderer>().material = TelegrabObject.TelegrabMaterial;

            grabbedGameObject.transform.parent = null;
            grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
            grabTimer = 0;
            //grabbedGameObject = null;
            grabbed = false;
            //isHighlighted = false;

        }
        RaycastHit hitInfo;
        if (Physics.Raycast(LookAt.position, LookAt.forward, out hitInfo, viewDistance, Moveable) && !grabbed&&canTeleGrab)
        {

            Debug.DrawLine(LookAt.position, hitInfo.point);
            if (!grabbed)
            {
                //isHighlighted = true;
                    //hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = grabbableMat;
                if (Input.GetKeyDown(KeyCode.V))
                {
                    grabbedGameObject = hitInfo.transform.gameObject;
                    TelegrabObject = hitInfo.transform.gameObject.GetComponent<TelegrabObject>();
                    grabbedGameObject.GetComponent<MeshRenderer>().material = grabbedMat;
                    grabbedGameObject.transform.parent = GrabPosition;
                    grabbedGameObject.transform.position = GrabPosition.position;
                    grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedGameObject.transform.rotation = Quaternion.identity;
                    grabbed = true;

                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Input.GetKeyDown(KeyCode.Escape)&&Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;

            }

        }
    }
    private void LateUpdate()
    {
        
    }
    bool GroundCheck()
    {
        if (Physics.CheckSphere(fox.position,boxSize.x,GroundLayerMask,QueryTriggerInteraction.Ignore))
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
        Gizmos.DrawCube(fox.position - fox.forward * maxDistance, boxSize);
    }
    IEnumerator disableGravity()
    {
        enableGravity = false;
        yield return new WaitForSeconds(0.2f);
        //turn off jumping
        enableGravity = true;
    }
    bool WaterCheck()
    {
        if (Physics.CheckSphere(fox.position, boxSize.x, WaterLayerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}