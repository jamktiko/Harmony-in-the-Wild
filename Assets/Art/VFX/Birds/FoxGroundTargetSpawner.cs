using System.Collections.Generic;
using UnityEngine;

public class FoxGroundTargetSpawner : MonoBehaviour
{
    [Header("Ground Target Settings")]
    public int maxGroundTargets = 10;      // Max number of pooled ground targets
    public float groundCheckDistance = 10f; // How far from the fox to place ground targets
    public LayerMask groundLayer;         // LayerMask to detect valid ground placement
    [SerializeField] private Vector3 groundTargetColliderSize = Vector3.one;
    [SerializeField] private bool spawnOnUpdate;

    [Header("References")]
    public Collider groundTriggerCollider; // Assign the fox�s ground trigger collider in the Inspector
    public Transform groundTargetContainer; // Parent object for hierarchy cleanliness

    private Queue<GameObject> groundTargetPool = new Queue<GameObject>(); // Object pool
    private HashSet<GameObject> activeTargets = new HashSet<GameObject>(); // Track active targets

    private void Start()
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
        PlaceGroundTargets();
    }


    private void Update()
    {
        if (spawnOnUpdate)
        {
            PlaceGroundTargets();
        }
    }

    private void InitializeObjectPool()
    {
        for (int i = 0; i < maxGroundTargets; i++)
        {
            GameObject target = new GameObject("lb_groundTarget");
            var collider = target.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = groundTargetColliderSize;
            target.tag = "lb_groundTarget";
            target.SetActive(false);
            target.transform.SetParent(groundTargetContainer);
            groundTargetPool.Enqueue(target);
        }
    }

    private void PlaceGroundTargets()
    {
        Vector3 foxPosition = transform.position;
        activeTargets.Clear();

        int spawnAttempts = 5;
        while (groundTargetPool.Count > 0 && spawnAttempts > 0)
        {
            Vector3 randomPos = GetRandomGroundPosition(foxPosition);
            if (randomPos == Vector3.zero)
            {
                spawnAttempts--;
                continue;
            }

            // Actually taking object from pool
            GameObject target = GetObjectFromPool();

            target.transform.position = randomPos;
            target.SetActive(true);
        }
    }

    private Vector3 GetRandomGroundPosition(Vector3 center)
    {
        for (int i = 0; i < 10; i++) // Try multiple times to find a valid ground point
        {
            Vector3 randomPos = center + new Vector3(
                Random.Range(-groundCheckDistance, groundCheckDistance),
                5, // Start above ground
                Random.Range(-groundCheckDistance, groundCheckDistance)
            );

            RaycastHit hit;
            float rayLength = 20f;

            // Visualizing raycast
            Debug.DrawLine(randomPos, randomPos + Vector3.down * rayLength, Color.blue);

            // Do a raycast to check if the position is valid
            if (Physics.Raycast(randomPos, Vector3.down, out hit, rayLength, groundLayer))
            {
                return hit.point;
            }
        }
        return Vector3.zero; // No valid position found
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != groundTriggerCollider) return; // Ensure it's the correct collider
        if (other.CompareTag("lb_bird")) return;

        foreach (var target in activeTargets)
        {
            if (target.activeSelf && Vector3.Distance(target.transform.position, transform.position) > groundCheckDistance * 1.5f)
            {
                ReturnToPool(target);
            }
        }

        PlaceGroundTargets();
    }

    private GameObject GetObjectFromPool()
    {
        if (groundTargetPool.TryDequeue(out GameObject target))
        {
            activeTargets.Add(target);
            return target;
        }
        return null;
    }
    private void ReturnToPool(GameObject target)
    {
        target.SetActive(false);
        groundTargetPool.Enqueue(target); // Returning object to pool
    }
}
