using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berries : MonoBehaviour
{
    [SerializeField] bool interactable;
    [SerializeField] static int BerryCollectableCount;
    [SerializeField] private GameObject interactionIndicator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Trigger")
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
    private void CollectBerry() 
    {
        if (interactable)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(95f, 0.5f)).Append(transform.DOScale(50f, 0.5f)).OnComplete(() =>
            {
                interactionIndicator.SetActive(false);

                BerryCollectableCount++;
                Destroy(gameObject);
            }); ;
        }
    }
    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPerformedThisFrame())
        {
            CollectBerry();
        }
    }
}
