using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SnowDiving : MonoBehaviour, IAbility
{
    public static SnowDiving Instance;

    public float SnowDiveSpeed = 15f;
    public float ClimbingSpeed = 20f;
    public float PointDistance = 0.5f;

    [SerializeField] private VisualEffect _snowDiveVFX;

    private int _onEnableSnowDiveID;
    private bool _isClimbing;
    private List<Transform> _movementPoints = new List<Transform>();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one SnowDiving ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _onEnableSnowDiveID = Shader.PropertyToID("onSnowDive");
    }
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.SnowDiving, this);
    }

    public void Activate()
    {
        SnowDive();
        Debug.Log("Activate called");

        if (Physics.Raycast(FoxMovement.Instance.CameraPosition.position, FoxMovement.Instance.CameraPosition.forward, out RaycastHit hit, 50f, FoxMovement.Instance.ClimbWallLayerMask))
        {
            Debug.DrawLine(FoxMovement.Instance.CameraPosition.position, hit.point);
            ClimbSnowWall(hit);
            //Debug.Log("if in Activate called and it hit: " + hit);
        }
    }
    private void SnowDive()
    {
        if (FoxMovement.Instance.IsGrounded())
        {
            //snowDiveVFX.SendEvent(onEnableSnowDiveID);

            FoxMovement.Instance.PlayerAnimator.SetBool("isGliding", false);
        }
    }

    private void ClimbSnowWall(RaycastHit hit)
    {
        Debug.Log("climbsnowwall called");
        if (!_isClimbing)
        {
            _isClimbing = true;
            _movementPoints.Clear();

            //climbing animation here (later will make more code for this)
            _snowDiveVFX.transform.position = FoxMovement.Instance.transform.Find("SnowDiveVFXPosition").position;
            _snowDiveVFX.SendEvent(_onEnableSnowDiveID);

            AudioManager.Instance.PlaySound(AudioName.AbilitySnowDive, transform);

            Transform movPointsParent = hit.transform.GetChild(0);

            foreach (Transform child in movPointsParent.transform)
            {
                _movementPoints.Add(child);
            }

            FindClosestPoint();

            StartCoroutine(MoveObject());
        }
    }
    private void FindClosestPoint()
    {
        Transform closestChild = null;
        float closestDistance = float.MaxValue;
        Vector3 playerPosition = FoxMovement.Instance.gameObject.transform.position;

        foreach (Transform child in _movementPoints)
        {
            float distance = Vector3.Distance(playerPosition, child.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestChild = child;
            }
        }

        int targetIndex = _movementPoints.IndexOf(closestChild);

        if (targetIndex > 0)
        {
            _movementPoints.RemoveRange(0, targetIndex);
        }

        //Debug.Log($"Closest child to the player is: {closestChild.name} with index {targetIndex}");
    }

    IEnumerator MoveObject()
    {
        FoxMovement.Instance.PlayerAnimator.SetBool("isSnowDiving", true);
        //turn off object to move it
        FoxMovement.Instance.Rb.isKinematic = true;
        FoxMovement.Instance.Rb.useGravity = false;

        //move towards point by list index, stop when gone through all items in the list
        for (int i = 0; i < _movementPoints.Count; i++)
        {
            //keep moving object towards the point while it's far from it
            while (Vector3.Distance(_movementPoints[i].position, FoxMovement.Instance.gameObject.transform.position) > PointDistance)
            {
                //Vector3 direction = (movementPoints[i].position - FoxMovement.instance.gameObject.transform.position).normalized;
                //FoxMovement.instance.gameObject.transform.Translate(direction * climbingSpeed * Time.deltaTime, Space.World);
                FoxMovement.Instance.gameObject.transform.position = Vector3.MoveTowards(FoxMovement.Instance.gameObject.transform.position, _movementPoints[i].position, ClimbingSpeed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }

        //turn the object back on after moving it through all points in the list
        _movementPoints.Clear();
        FoxMovement.Instance.Rb.isKinematic = false;
        FoxMovement.Instance.Rb.useGravity = true;
        _isClimbing = false;
        FoxMovement.Instance.PlayerAnimator.SetBool("isSnowDiving", false);
    }
}
