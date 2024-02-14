using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Reindeer : MonoBehaviour
{
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private Vector3 currentDestination;
    private int currentDestinationIndex;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestination = destinations[0].position;

        StartCoroutine(WalkToDestination());
    }

    private void SetNewDestination()
    {
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
        bool nearDestination = false;

        SetNewDestination();

        SetTurnAnimation();

        agent.speed = 0.7f;

        while (!nearDestination)
        {
            agent.SetDestination(currentDestination);

            if (Vector3.Distance(transform.position, currentDestination) < 0.5f)
            {
                nearDestination = true;
            }

            yield return null;
        }

        StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {
        animator.SetTrigger("idle");

        yield return new WaitForSeconds(3f);

        StartCoroutine(WalkToDestination());
    }

    private void SetTurnAnimation()
    {
        float direction = transform.position.x - currentDestination.x;

        animator.SetTrigger("walk");
        animator.SetFloat("turn", direction);
    }
}
