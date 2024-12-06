using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PineconesCollectable : MonoBehaviour
{
    [SerializeField] bool interactable;
    [SerializeField] static int PineCollectableCount;
    [SerializeField] private GameObject interactionIndicator;

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
    private void CollectCone()
    {
        if (interactable)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(1.3f, 0.5f)).Append(transform.DOScale(0f, 0.5f)).OnComplete(() =>
            {
                interactionIndicator.SetActive(false);

                PlayerManager.instance.PineCones++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.instance.AchievementProgressPinecone("stat_3");
                }
                PlayerManager.instance.BerryData[transform.name] = false;
                gameObject.SetActive(false);
            }); 
            //transform.DOScale(0f, 0.5f).OnComplete(() =>
            //{
            //    CollectableCount++;
            //    Destroy(gameObject);
            //});
        }
    }
    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPerformedThisFrame())
        {
            CollectCone();
        }
    }
}