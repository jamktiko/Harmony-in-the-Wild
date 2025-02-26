using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCMovement : MonoBehaviour
{
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private DialogueQuestNPCs character;

    [Header("Movement Config")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float maxDistanceToPlayer = 10f;

    [Header("Nav Mesh Movement Config")]

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
        if (QuestManager.instance.CheckQuestState(questSO.id) == QuestState.CAN_FINISH)
        {
            transform.position = destinations[destinations.Count - 1].position;
        }

        else
        {
            defaultSpeed = moveSpeed;
            currentDestination = destinations[0].position;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnStartMovingQuestNPC += EnableMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartMovingQuestNPC -= EnableMovement;
    }

    private void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, player.position) <= maxDistanceToPlayer;
    }

    private void EnableMovement(DialogueQuestNPCs characterToMove)
    {
        if (character == characterToMove)
        {
            StartCoroutine(WalkToDestination());

            Debug.Log("Start quest NPC movement...");
        }
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (currentDestinationIndex < destinations.Count - 1)
        {
            currentDestinationIndex++;

            currentDestination = destinations[currentDestinationIndex].position;

            StartCoroutine(WalkToDestination());
        }

        else
        {
            GameEventsManager.instance.questEvents.ReachWhaleDestination();
        }
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
        yield return new WaitUntil(() => playerIsNear);

        StartCoroutine(WalkToDestination());
    }
}
