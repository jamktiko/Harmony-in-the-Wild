using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("Idle Duration Config")]
    [SerializeField] private float minDuration = 2f;
    [SerializeField] private float maxDuration = 10f;

    [Header("Needed References")]
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private Vector3 currentDestination;
    private int currentDestinationIndex;
    private float defaultSpeed;
    private bool playerIsNear;
    private Coroutine idleCoroutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestination = destinations[0].position;
        defaultSpeed = agent.speed;

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
            currentDestinationIndex = 0;
        }

        currentDestination = destinations[currentDestinationIndex].position;
    }

    private IEnumerator WalkToDestination()
    {
        // make sure there is no overlapping idle coroutines going
        if(idleCoroutine != null)
        {
            idleCoroutine = null;
        }

        bool nearDestination = false;

        SetNewDestination();

        SetTurnAnimation();

        while (!nearDestination && !playerIsNear)
        {
            agent.SetDestination(currentDestination);

            if (Vector3.Distance(transform.position, currentDestination) < 1.5f)
            {
                nearDestination = true;
            }

            yield return null;
        }

        idleCoroutine = StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {
        agent.speed = 0;
        animator.SetTrigger("idle");

        yield return new WaitForSeconds(Random.Range(minDuration, maxDuration));

        //yield return new WaitUntil(playerIsNear == false);

        if (!playerIsNear)
        {
            agent.speed = defaultSpeed;
            StartCoroutine(WalkToDestination());
        }
    }

    private void SetTurnAnimation()
    {
        float direction = transform.position.x - currentDestination.x;

        animator.SetTrigger("walk");
        animator.SetFloat("turn", direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;

            if(idleCoroutine == null)
            {
                StartCoroutine(Idle());
            }        
        }
    }
}
