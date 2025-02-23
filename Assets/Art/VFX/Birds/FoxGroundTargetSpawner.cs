using UnityEngine;
using System.Collections.Generic;

public class FoxGroundTargetSpawner : MonoBehaviour
{
    [Header("Ground Target Settings")]
    public GameObject groundTargetPrefab;  // Prefab for ground target
    public int maxGroundTargets = 10;      // Max number of pooled ground targets
    public float groundCheckDistance = 10f; // How far from the fox to place ground targets
    public LayerMask groundLayer;         // LayerMask to detect valid ground placement

    [Header("References")]
    public Collider groundTriggerCollider; // Assign the fox’s ground trigger collider in the Inspector
    public Transform groundTargetContainer; // Parent object for hierarchy cleanliness

    private List<GameObject> groundTargetPool = new List<GameObject>(); // Object pool
    private HashSet<Vector3> activeTargetPositions = new HashSet<Vector3>(); // Track active target positions

    void Start()
    {
        if (groundTriggerCollider == null)
        {
            Debug.LogError("Ground Target Collider not assigned in FoxGroundTargetSpawner!");
            return;
        }

        groundTriggerCollider.isTrigger = true; // Ensure it's a trigger

        if (groundTargetContainer == null)
        {
            // If no container is assigned, create one dynamically
            GameObject container = new GameObject("GroundTargetContainer");
            groundTargetContainer = container.transform;
        }

        InitializeObjectPool();
    }

    void InitializeObjectPool()
    {
        for (int i = 0; i < maxGroundTargets; i++)
        {
            GameObject target = Instantiate(groundTargetPrefab);
            target.SetActive(false);
            target.transform.SetParent(groundTargetContainer);
            groundTargetPool.Add(target);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != groundTriggerCollider) return; // Ensure it's the correct collider
        if (other.CompareTag("lb_bird")) return; // Ignore birds

        PlaceGroundTargets();
    }

    void PlaceGroundTargets()
    {
        Vector3 foxPosition = transform.position;
        activeTargetPositions.Clear();

        for (int i = 0; i < maxGroundTargets; i++)
        {
            Vector3 randomPos = GetRandomGroundPosition(foxPosition);

            if (randomPos != Vector3.zero && !activeTargetPositions.Contains(randomPos))
            {
                GameObject target = GetPooledGroundTarget();
                if (target != null)
                {
                    target.transform.position = randomPos;
                    target.SetActive(true);
                    activeTargetPositions.Add(randomPos);
                }
            }
        }
    }

    Vector3 GetRandomGroundPosition(Vector3 center)
    {
        for (int i = 0; i < 10; i++) // Try multiple times to find a valid ground point
        {
            Vector3 randomPos = center + new Vector3(
                Random.Range(-groundCheckDistance, groundCheckDistance),
                5, // Start above ground
                Random.Range(-groundCheckDistance, groundCheckDistance)
            );

            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit, 20f, groundLayer))
            {
                return hit.point;
            }
        }
        return Vector3.zero; // No valid position found
    }

    GameObject GetPooledGroundTarget()
    {
        foreach (GameObject target in groundTargetPool)
        {
            if (!target.activeInHierarchy)
                return target;
        }
        return null; // No available targets (all in use)
    }

    void OnTriggerExit(Collider other)
    {
        if (other != groundTriggerCollider) return; // Ensure it's the correct collider
        if (other.CompareTag("lb_bird")) return;

        // Recycle old targets when the fox moves away
        foreach (GameObject target in groundTargetPool)
        {
            if (target.activeSelf && Vector3.Distance(target.transform.position, transform.position) > groundCheckDistance * 1.5f)
            {
                target.SetActive(false);
            }
        }
    }
}
