using System.Collections;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool isFrozen;

    [Header("Freeze Config")]
    [SerializeField] private float freezeTime;

    //TODO: Rename both of these once purpose is more clear. Now naming implies they're booleans.
    [Header("Needed References")]
    [SerializeField] private GameObject isFrozenEffect;
    [SerializeField] private GameObject canBeFrozen;

    private Rigidbody rb;
    private Vector3 targetPosition;

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rb);
    }
   
    public void FreezeObject()
    {
        Debug.Log(gameObject.name + " has been frozen.");
        isFrozen = true;

        if(rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        canBeFrozen.SetActive(false);
        isFrozenEffect.SetActive(true);

        StartCoroutine(FreezeCooldown());
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(freezeTime);

        isFrozen = false;

        if(rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        canBeFrozen.SetActive(true);
        isFrozenEffect.SetActive(false);

        Debug.Log(gameObject.name + " has been unfrozen.");
    }
}

//Note: What does this do? Where is it used?
[System.Serializable]
public enum MoveDirection
{
    Forward,
    Backwards
}
