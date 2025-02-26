using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    [Header("Freeze State")]
    public bool IsFrozen;

    [Header("Freeze Config")]
    [SerializeField] private float _freezeTime;

    //TODO: Rename both of these once purpose is more clear. Now naming implies they're booleans.
    [Header("Needed References")]
    [SerializeField] private GameObject _canBeFrozen;
    [SerializeField] private Material _newFrozenMaterial;
    [SerializeField] private AudioSource _freeze;
    [SerializeField] private AudioClip _freezeClip;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rb);

        if (_canBeFrozen == null)
        {
            _canBeFrozen = transform.Find("Effects").Find("FreezableRock").gameObject;

            if (_canBeFrozen == null)
            {
                Debug.Log("Couldn't find freezable effect reference for " + gameObject.name);
            }
        }
    }

    private void OnEnable()
    {
        if (originalMaterials.Count > 0)
        {
            SwapMaterials(false);
        }

        IsFrozen = false;
        _canBeFrozen.SetActive(true);
    }

    public void FreezeObject()
    {
        Debug.Log(gameObject.name + " has been frozen.");
        IsFrozen = true;

        if (_freeze != null)
        {
            _freeze.PlayOneShot(_freezeClip);
        }

        else
        {
            Debug.LogWarning("No Audio Source assigned for " + gameObject.name + "; no freezing audio played.");
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }

        if (_canBeFrozen != null)
        {
            _canBeFrozen.SetActive(false);
        }

        // Material swapping
        SwapMaterials(true);

        StartCoroutine(FreezeCooldown());
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(_freezeTime);

        IsFrozen = false;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        if (_canBeFrozen != null)
            _canBeFrozen.SetActive(true);

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
                    childRenderer.material = _newFrozenMaterial;
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