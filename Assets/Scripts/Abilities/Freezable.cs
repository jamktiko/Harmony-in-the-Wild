using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool isFreezed;

    [Header("Freeze Config")]
    [SerializeField] private float freezeTime;

    [Header("Movement Config")]
    [SerializeField] private bool isMovable;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private MoveDirection direction;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;

    private Rigidbody rb;
    private Vector3 targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        targetPosition = endPosition;
    }

    private void FixedUpdate()
    {
        if (!isFreezed && isMovable)
        {
            Move();
        }
    }

    private void Move()
    {
        switch (direction)
        {
            case MoveDirection.Forward:
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));

                if(Vector3.Distance(transform.position, endPosition) < 5)
                {
                    targetPosition = startPosition;
                    direction = MoveDirection.Backwards;
                }

                break;

            case MoveDirection.Backwards:
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));

                if (Vector3.Distance(transform.position, startPosition) < 5)
                {
                    targetPosition = endPosition;
                    direction = MoveDirection.Forward;
                }

                break;
        }
    }

    public void Freeze()
    {
        Debug.Log(gameObject.name + " has been freezed.");
        isFreezed = true;

        StartCoroutine(FreezeCooldown());
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(freezeTime);

        isFreezed = false;

        Debug.Log(gameObject.name + " has been unfreezed.");
    }
}

[System.Serializable]
public enum MoveDirection
{
    Forward,
    Backwards
}
