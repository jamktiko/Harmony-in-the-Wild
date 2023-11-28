using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool isFreezed;

    [Header("Freeze Config")]
    [SerializeField] private float freezeTime;

    private Rigidbody rb;
    private Vector3 targetPosition;

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rb);
    }
   
    public void Freeze()
    {
        Debug.Log(gameObject.name + " has been freezed.");
        isFreezed = true;

        if(rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        StartCoroutine(FreezeCooldown());
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(freezeTime);

        isFreezed = false;

        if(rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        Debug.Log(gameObject.name + " has been unfreezed.");
    }
}

[System.Serializable]
public enum MoveDirection
{
    Forward,
    Backwards
}
