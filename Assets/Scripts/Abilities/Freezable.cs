using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool isFreezed;

    [Header("Freeze Config")]
    [SerializeField] private float freezeTime;

    [Header("Needed References")]
    [SerializeField] private GameObject isFreezedEffect;
    [SerializeField] private GameObject canBeFreezed;

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

        canBeFreezed.SetActive(false);
        isFreezedEffect.SetActive(true);

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

        canBeFreezed.SetActive(true);
        isFreezedEffect.SetActive(false);

        Debug.Log(gameObject.name + " has been unfreezed.");
    }
}

[System.Serializable]
public enum MoveDirection
{
    Forward,
    Backwards
}
