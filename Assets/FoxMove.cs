using HeneGames.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    float rotation;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask GroundLayerMask;
    public LayerMask WaterLayerMask;
    public float jumpforce = 10f;
    public float timer = 0f;
    [SerializeField] bool enableGravity=true;
    [SerializeField] float jumpSpeed = 10f;
    Vector3 Jump = new Vector3(0, 0, 0);
    bool sprinting;
    public bool canSwim = false;
    [SerializeField] GameObject diaUI;
    // Start is called before the first frame update
    void Start()
    {
        //diaUI = Resources.FindObjectsOfTypeAll<DialogueUI>().First().gameObject;
        //diaUI.SetActive(true);
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
        if (GroundCheck())
        {
            animator.SetBool("Jump", false);

            //if (Horizontal == 0 && Vertical == 0 && cameraMove.X == 0)
            //{
            //    animator.SetBool("RunLeft", false);
            //    animator.SetBool("RunRight", false);
            //    animator.SetBool("Run", false);
            //    animator.SetBool("RunBack", false);
            //    animator.SetBool("Idle", true);
            //    timer++;
            //    if (timer > 400 && animator.GetBool("CanSit") != true)
            //    {
            //        animator.SetBool("CanSit", true);
            //    }
            //}
            if (Input.GetKey(KeyCode.LeftShift)&&!sprinting)
            {
                sprinting = true;
                animator.speed = 1.4f;
                Speed = 5;
            }
            else
            {
                Speed = 4;
                animator.speed = 1f;
                sprinting = false;
            }
            if (!WaterCheck())
            {
                animator.speed = 1;
            }
            if (Vertical==0&&Horizontal==0)
            {
                foreach (AnimatorControllerParameter item in animatorBools)
                {
                    animator.SetBool(item.name, false);
                }
                animator.SetBool("Idle", true);
                timer++;
                if (timer > 400 && animator.GetBool("CanSit") != true)
                {
                    animator.SetBool("Idle", false);
                    animator.SetBool("CanSit", true);
                }
            }
            else if (Vertical > 0 || Horizontal > 0 || Horizontal < 0)
            {
                if (cameraMove.X == 0)
                {
                    timer = 0;
                    animator.SetBool("RunLeft", false);
                    animator.SetBool("RunRight", false);
                    animator.SetBool("CanSit", false);
                    animator.SetBool("RunBack", false);
                    animator.SetBool("Idle", false);
                    animator.SetBool("Run", true);
                }

                else if (cameraMove.X > 0.05)
                {
                    timer = 0;
                    animator.SetBool("Run", false);
                    animator.SetBool("CanSit", false);
                    animator.SetBool("RunLeft", false);
                    animator.SetBool("RunRight", true);
                }
                else if (cameraMove.X < -0.05)
                {
                    timer = 0;
                    animator.SetBool("Run", false);
                    animator.SetBool("CanSit", false);
                    animator.SetBool("RunLeft", true);
                    animator.SetBool("RunRight", false);
                }
            }
            else if (Vertical < 0)
            {
                timer = 0;
                animator.SetBool("CanSit", false);
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetBool("RunBack", true);
            }
        }
        else if (WaterCheck())
        {
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                animator.SetBool(item.name, false);
            }
            animator.SetBool("Run", true);
            animator.speed = 0.5f;
        }
        else if (!GroundCheck())
        {
            foreach (AnimatorControllerParameter item in animatorBools)
            {
                animator.SetBool(item.name, false);
            }
            animator.SetBool("Jump",true);
        }
        //else
        //{
        //    animator.SetBool("RunLeft", false);
        //    animator.SetBool("RunRight", false);
        //}
        
        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Vector3 MovementJump = new Vector3(0, 0, 0);
        //Movement.Normalize();
        if (GroundCheck()&&Input.GetButton("Jump"))
        {
            MovementJump.y = jumpforce;
            Jump = MovementJump;
            Debug.Log("hi");
            
            enableGravity = false;
            StartCoroutine(disableGravity());
        }
        else if (enableGravity)
        {
            Jump.y -= gravity * Time.deltaTime;
        }
        Controller.Move(Jump * Time.deltaTime*jumpSpeed);

        Controller.Move(Movement*Speed);
        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);
            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        }
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
        animator.SetBool("Jump", false);
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