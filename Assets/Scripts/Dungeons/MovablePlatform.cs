using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [Header("Movement Type")]
    [SerializeField] private bool circularMovement;

    [Header("Movement Config")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private MoveDirection direction;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    private Rigidbody rb;
    private Vector3 targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = endPosition.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        switch (direction)
        {
            case MoveDirection.Forward:
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));

                if (Vector3.Distance(transform.position, endPosition.position) < 5)
                {
                    targetPosition = startPosition.position;
                    direction = MoveDirection.Backwards;
                }

                break;

            case MoveDirection.Backwards:
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));

                if (Vector3.Distance(transform.position, startPosition.position) < 5)
                {
                    targetPosition = endPosition.position;
                    direction = MoveDirection.Forward;
                }

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player on board");

            //other.GetComponent<FoxMove>().enabled = false;
            other.GetComponent<CharacterController>().enabled = false;

            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            Debug.Log("player not on board");
        }
    }
}
