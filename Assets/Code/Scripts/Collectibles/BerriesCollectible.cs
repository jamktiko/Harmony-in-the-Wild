using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Berries : MonoBehaviour
{
    [SerializeField] bool interactable;
    [SerializeField] static int BerryCollectableCount;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private TMP_Text notifText;

    private bool hasBeenCollected = false;

    private void Start()
    {
        notifText=GameObject.Find("CollectibleNotification").GetComponent<TMP_Text>();
    }


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
            FoxMovement.instance.playerAnimator.SetBool(FoxAnimation.Parameter.isCollectingBerry, true);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(95f, 0.5f)).Append(transform.DOScale(50f, 0.5f)).OnComplete(() =>
            {
                interactionIndicator.SetActive(false);

                FoxMovement.instance.playerAnimator.SetBool(FoxAnimation.Parameter.isCollectingBerry, false);

                PlayerManager.instance.Berries++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.instance.AchievementProgressBerry("stat_2");
                }
                PlayerManager.instance.BerryData[transform.parent.name] = false;
                CollectibleNotification(notifText, "Berries");
                Invoke("CollectibleNotificationDisappear",7f);
                gameObject.SetActive(false);
            });
        }
    }
    private void CollectibleNotification(TMP_Text notifText, string CollectibleType)
    {
        notifText.text = CollectibleType + " collected: " + (CollectibleType.Contains("Berries") ? PlayerManager.instance.Berries : PlayerManager.instance.PineCones);
    }
    private void CollectibleNotificationDisappear()
    {
        notifText.text = "";
    }
    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPerformedThisFrame() && interactable && !hasBeenCollected)
        {
            hasBeenCollected = true;
            CollectBerry();
        }
    }
}
