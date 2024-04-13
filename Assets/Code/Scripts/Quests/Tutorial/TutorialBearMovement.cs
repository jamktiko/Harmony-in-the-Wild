using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialBearMovement : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] private Transform targetTransform;

    [Header("Other Needed References")]
    [SerializeField] private QuestScriptableObject tutorialQuestSO;
    [SerializeField] private Animator animator;

    private bool canMove;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += CheckMovementActivation;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= CheckMovementActivation;
    }

    private void Update()
    {
        if (canMove)
        {
            agent.SetDestination(targetTransform.position);

            if (Vector3.Distance(transform.position, targetTransform.position) < 1.5f)
            {
                gameObject.SetActive(false);
            }
        }   
    }

    private void CheckMovementActivation(string questId)
    {
        if(questId == tutorialQuestSO.id)
        {
            EnableMovement();
        }
    }

    private void EnableMovement()
    {
        if (QuestManager.instance.GetQuestById(tutorialQuestSO.id).GetCurrentQuestStepIndex() == 3)
        {
            canMove = true;
            animator.SetTrigger("walk");
        }
    }
}
