using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReindeerSpeedChange : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    public void IncreaseSpeedAfterTurn()
    {
        agent.speed = 1f;
    }
}
