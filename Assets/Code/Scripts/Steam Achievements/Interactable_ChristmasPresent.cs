using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableChristmasPresent : MonoBehaviour
{
    [FormerlySerializedAs("isActive")] [SerializeField] bool _isActive = false;
    [FormerlySerializedAs("wasUsed")] [SerializeField] public bool WasUsed = false; //Note: does this need to be public? private and method to pass value
    [FormerlySerializedAs("hats")] [SerializeField] List<GameObject> _hats;

    void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive)
        {
            foreach (GameObject hat in _hats)
            {
                hat.SetActive(true);
            }

            SteamManager.Instance.UnlockAchievement("CHRIMIS_ACH");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _isActive = false;
        }
    }
}
