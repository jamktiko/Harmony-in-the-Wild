using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool isFrozen;

    [Header("Freeze Config")]
    [SerializeField] private float freezeTime;

    //TODO: Rename both of these once purpose is more clear. Now naming implies they're booleans.
    [Header("Needed References")]
    [SerializeField] private GameObject canBeFrozen;
    [SerializeField] private Material newFrozenMaterial;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rb);

        if(canBeFrozen == null)
        {
            canBeFrozen = transform.Find("Effects").Find("FreezableRock").gameObject;

            if(canBeFrozen == null)
            {
                Debug.Log("Couldn't find freezable effect reference for " + gameObject.name);
            }
        }
    }

    public void FreezeObject()
    {
        Debug.Log(gameObject.name + " has been frozen.");
        isFrozen = true;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (canBeFrozen != null)
            canBeFrozen.SetActive(false);

        // Material swapping
        SwapMaterials(true);

        StartCoroutine(FreezeCooldown());
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(freezeTime);

        isFrozen = false;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        if (canBeFrozen != null)
            canBeFrozen.SetActive(true);
        
        SwapMaterials(false);

        Debug.Log(gameObject.name + " has been unfrozen.");
    }

    private void SwapMaterials(bool toFrozen)
    {
        // Assuming "Rocks" is a direct child of the game object this script is attached to.
        Transform rocksParent = transform.Find("Rocks");
        if (rocksParent == null)
        {
            Debug.LogError("[SwapMaterials] 'Rocks' object not found in hierarchy.");
            return;
        }

        foreach (Transform child in rocksParent)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                if (toFrozen)
                {
                    // Store the original material if not already stored.
                    if (!originalMaterials.ContainsKey(childRenderer))
                    {
                        originalMaterials.Add(childRenderer, childRenderer.material);
                        Debug.Log($"[SwapMaterials] Stored original material for {child.name}.");
                    }

                    // Apply the new material.
                    childRenderer.material = newFrozenMaterial;
                    Debug.Log($"[SwapMaterials] Applied new material to {child.name}.");
                }
                else
                {
                    // Revert to the original material if stored.
                    if (originalMaterials.TryGetValue(childRenderer, out Material originalMat))
                    {
                        childRenderer.material = originalMat;
                        Debug.Log($"[SwapMaterials] Reverted to original material for {child.name}.");
                    }
                    else
                    {
                        Debug.LogWarning($"[SwapMaterials] No original material found for {child.name}. Could not revert.");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[SwapMaterials] No Renderer found on {child.name}.");
            }
        }
    }
}