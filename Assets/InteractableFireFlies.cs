using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableFireFlies : MonoBehaviour
{
    [SerializeField] bool interactable;
    [SerializeField] static int PineCollectableCount;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private Animator PlayerAnimator;

    private bool hasBeenCollected = false;

    private void Start()
    {
        PlayerAnimator=FoxMovement.instance.gameObject.GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            interactable = false;
        }
    }
    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPerformedThisFrame() && interactable)
        {

            PlayAnimation();
        }
    }
    private void PlayAnimation() 
    {
        PlayerAnimator.Play("PL_Playful2_ANI");
        
    }
}
