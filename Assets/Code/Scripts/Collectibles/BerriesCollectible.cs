using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            FoxMovement.instance.playerAnimator.SetBool("isCollectingBerry", true);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(95f, 0.5f)).Append(transform.DOScale(50f, 0.5f)).OnComplete(() =>
            {
                interactionIndicator.SetActive(false);

                FoxMovement.instance.playerAnimator.SetBool("isCollectingBerry", false);

                PlayerManager.instance.Berries++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.instance.AchievementProgressBerry("stat_2");
                }
                PlayerManager.instance.BerryData[transform.parent.name] = false;
                gameObject.SetActive(false);
            });
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
