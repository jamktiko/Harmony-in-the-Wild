using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_ChristmasPresent : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //Note: does this need to be public? private and method to pass value
    [SerializeField] List<GameObject> hats;

    void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive)
        {
            foreach(GameObject hat in hats)
            {
                hat.SetActive(true);
            }

            SteamManager.instance.UnlockAchievement("CHRIMIS_ACH");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isActive = false;
        }
    }
}
