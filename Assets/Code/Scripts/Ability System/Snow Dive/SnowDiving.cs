using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SnowDiving : MonoBehaviour, IAbility
{
    public static SnowDiving instance;

    public float snowDiveSpeed = 15f;
    public float climbingSpeed = 20f;
    public float pointDistance = 0.5f;

    [SerializeField] private VisualEffect snowDiveVFX;

    private int onEnableSnowDiveID;
    private bool isClimbing;
    private List<Transform> movementPoints = new List<Transform>();
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one SnowDiving ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        onEnableSnowDiveID = Shader.PropertyToID("onSnowDive");
    }
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.SnowDiving, this);
    }

    public void Activate()
    {
        SnowDive();
        Debug.Log("Activate called");

        if (Physics.Raycast(FoxMovement.instance.cameraPosition.position, FoxMovement.instance.cameraPosition.forward, out RaycastHit hit, 50f, FoxMovement.instance.climbWallLayerMask))
        {
            Debug.DrawLine(FoxMovement.instance.cameraPosition.position, hit.point);
            ClimbSnowWall(hit);
            //Debug.Log("if in Activate called and it hit: " + hit);
        }
    }
    private void SnowDive()
    {
        if (FoxMovement.instance.IsGrounded())
        {
            //snowDiveVFX.SendEvent(onEnableSnowDiveID);

            FoxMovement.instance.playerAnimator.SetBool("isGliding", false);
        }
    }

    private void ClimbSnowWall(RaycastHit hit)
    {
        Debug.Log("climbsnowwall called");
        if (!isClimbing)
        {
            isClimbing = true;
            movementPoints.Clear();

            //climbing animation here (later will make more code for this)
            snowDiveVFX.transform.position = FoxMovement.instance.transform.Find("SnowDiveVFXPosition").position;
            snowDiveVFX.SendEvent(onEnableSnowDiveID);

            AudioManager.instance.PlaySound(AudioName.Ability_SnowDive, transform);

            Transform movPointsParent = hit.transform.GetChild(0);

            foreach (Transform child in movPointsParent.transform)
            {
                movementPoints.Add(child);
            }

            FindClosestPoint();

            StartCoroutine(MoveObject());
        }
    }
    private void FindClosestPoint()
    {
        Transform closestChild = null;
        float closestDistance = float.MaxValue;
        Vector3 playerPosition = FoxMovement.instance.gameObject.transform.position;

        foreach (Transform child in movementPoints)
        {
            float distance = Vector3.Distance(playerPosition, child.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestChild = child;
            }
        }

        int targetIndex = movementPoints.IndexOf(closestChild);

        if (targetIndex > 0)
        {
            movementPoints.RemoveRange(0, targetIndex);
        }
        
        //Debug.Log($"Closest child to the player is: {closestChild.name} with index {targetIndex}");
    }

    IEnumerator MoveObject()
    {
        FoxMovement.instance.playerAnimator.SetBool("isSnowDiving",true);
        //turn off object to move it
        FoxMovement.instance.rb.isKinematic = true;
        FoxMovement.instance.rb.useGravity = false;

        //move towards point by list index, stop when gone through all items in the list
        for (int i = 0; i < movementPoints.Count; i++)
        {
            //keep moving object towards the point while it's far from it
            while (Vector3.Distance(movementPoints[i].position, FoxMovement.instance.gameObject.transform.position) > pointDistance)
            {
                //Vector3 direction = (movementPoints[i].position - FoxMovement.instance.gameObject.transform.position).normalized;
                //FoxMovement.instance.gameObject.transform.Translate(direction * climbingSpeed * Time.deltaTime, Space.World);
                FoxMovement.instance.gameObject.transform.position = Vector3.MoveTowards(FoxMovement.instance.gameObject.transform.position, movementPoints[i].position, climbingSpeed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }

        //turn the object back on after moving it through all points in the list
        movementPoints.Clear();
        FoxMovement.instance.rb.isKinematic = false;
        FoxMovement.instance.rb.useGravity = true;
        isClimbing = false;
        FoxMovement.instance.playerAnimator.SetBool("isSnowDiving", false);
    }
}
