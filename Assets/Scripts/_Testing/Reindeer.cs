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
        animator.SetTrigger("walk");
        agent = GetComponent<NavMeshAgent>();
        currentDestination = destinations[0].position;
    }

    private void Update()
    {
        agent.SetDestination(currentDestination);

        if(Vector3.Distance(transform.position, currentDestination) < 0.5f)
        {
            if(currentDestinationIndex < destinations.Count - 1)
            {
                currentDestinationIndex++;
            }

            else
            {
                currentDestinationIndex = 0;
            }

            currentDestination = destinations[currentDestinationIndex].position;
        }
    }
}
