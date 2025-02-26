using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshNPCMovement : MonoBehaviour
{
    [Header("Quest Config")]
    [SerializeField] private DialogueQuestNPCs character;
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int questStepIndexToAllowMovement;

    [Header("Movement Config")]
    [SerializeField] private float maxDistanceToPlayer = 10f;
    [SerializeField] private List<Transform> destinations;

    [Header("Other Needed References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private Transform player;

    private Vector3 currentTargetDestination;
    private int currentTargetDestinationIndex;
    private bool canMove;
    private bool playerIsNear;
    private NavMeshAgent agent;
    private Coroutine idleCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        CheckInitialSpawnPoint();
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
        if (canMove && playerIsNear)
        {
            idleCoroutine = null;
            MoveNPC();
        }

        else if(canMove && !playerIsNear && idleCoroutine == null)
        {
            idleCoroutine = StartCoroutine(Idle());
        }

        CheckDistanceToPlayer();
    }

    private void MoveNPC()
    {
        agent.SetDestination(currentTargetDestination);

        if (Vector3.Distance(transform.position, currentTargetDestination) < 1.5f)
        {
            SetNewDestination();
        }
    }

    private void EnableMovement(DialogueQuestNPCs questCharacterToMove)
    {
        if(character == questCharacterToMove)
        {
            if (QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex() == questStepIndexToAllowMovement)
            {
                canMove = true;

                if (animator != null)
                {
                    animator.SetTrigger("walk");
                }

                else
                {
                    Debug.LogError($"No animator attached to { gameObject.name }!");
                }

                if (interactionIndicator != null)
                {
                    interactionIndicator.SetActive(false);
                }

                else
                {
                    Debug.LogError($"No interaction indicator attached to { gameObject.name }!");
                }
            }
        }
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (currentTargetDestinationIndex < destinations.Count - 1)
        {
            currentTargetDestinationIndex++;

            currentTargetDestination = destinations[currentTargetDestinationIndex].position;
        }

        else
        {
            GameEventsManager.instance.questEvents.ReachTargetDestinationToCompleteQuestStep(questSO.id);
            canMove = false;

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(true);
            }

            else
            {
                Debug.LogError($"No interaction indicator attached to { gameObject.name }!");
            }
        }
    }

    private void CheckInitialSpawnPoint()
    {
        if (QuestManager.instance.CheckQuestState(questSO.id) == QuestState.CAN_FINISH)
        {
            transform.position = destinations[destinations.Count - 1].position;
        }

        else
        {
            currentTargetDestination = destinations[0].position;
        }
    }

    private void CheckDistanceToPlayer()
    {
        playerIsNear = Vector3.Distance(transform.position, player.position) <= maxDistanceToPlayer;
    }

    private IEnumerator Idle()
    {
        // set animation to idle

        yield return new WaitUntil(() => playerIsNear);

        // set animation to move
    }
}