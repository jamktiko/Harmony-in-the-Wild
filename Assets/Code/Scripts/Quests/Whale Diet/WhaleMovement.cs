using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhaleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float maxDistanceToPlayer = 10f;

    [Header("Needed References")]
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Transform player;
    //[SerializeField] private Animator animator;

    private float defaultSpeed;
    private Vector3 currentDestination;
    private int currentDestinationIndex;
    private bool playerIsNear;
    private Coroutine idleCoroutine;

    private void Start()
    {
        defaultSpeed = moveSpeed;
        currentDestination = destinations[0].position;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnStartMovingWhale += EnableMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartMovingWhale -= EnableMovement;
    }

    private void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, player.position) <= maxDistanceToPlayer;
    }

    private void EnableMovement()
    {
        StartCoroutine(WalkToDestination());
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (currentDestinationIndex < destinations.Count - 1)
        {
            currentDestinationIndex++;
        }

        else
        {
            GameEventsManager.instance.questEvents.ReachWhaleDestination();
        }

        currentDestination = destinations[currentDestinationIndex].position;

        StartCoroutine(WalkToDestination());
    }

    private IEnumerator WalkToDestination()
    {
        // make sure there is no overlapping idle coroutines going
        if (idleCoroutine != null)
        {
            idleCoroutine = null;
        }

        yield return new WaitUntil(() => playerIsNear);

        bool nearDestination = false;

        while (!nearDestination && playerIsNear)
        {
            // move towards the target location
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            
            // rotate towards the target location
            Vector3 direction = (currentDestination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            if (Vector3.Distance(transform.position, currentDestination) < 1.5f)
            {
                nearDestination = true;
            }

            yield return null;
        }

        if (playerIsNear)
        {
            SetNewDestination();
        }

        else
        {
            idleCoroutine = StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle()
    {
        //animator.SetTrigger("idle");

        yield return new WaitUntil(() => playerIsNear);

        StartCoroutine(WalkToDestination());
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }*/
}
