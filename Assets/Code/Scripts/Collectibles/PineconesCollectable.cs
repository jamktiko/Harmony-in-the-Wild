using DG.Tweening;
using TMPro;
using UnityEngine;

public class PineconesCollectable : MonoBehaviour
{
    [SerializeField] bool interactable;
    [SerializeField] static int PineCollectableCount;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private TMP_Text notifText;

    private bool hasBeenCollected = false;

    private void Start()
    {
        notifText = GameObject.Find("CollectibleNotification").GetComponent<TMP_Text>();
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
    private void CollectCone()
    {
        if (interactable)
        {
            FoxMovement.instance.playerAnimator.SetBool("isCollectingPinecone", true);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(1.3f, 1.3f)).Append(transform.DOScale(0f, 1.3f)).OnComplete(() =>
            {
                interactionIndicator.SetActive(false);

                FoxMovement.instance.playerAnimator.SetBool("isCollectingPinecone", false);

                PlayerManager.instance.PineCones++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.instance.AchievementProgressPinecone("stat_3");
                }
                PlayerManager.instance.PineConeData[transform.name] = false;
                CollectibleNotification(notifText, "Pinecones");
                Invoke("CollectibleNotificationDisappear", 7f);
                gameObject.SetActive(false);
            });
            //transform.DOScale(0f, 0.5f).OnComplete(() =>
            //{
            //    CollectableCount++;
            //    Destroy(gameObject);
            //});
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

            CollectCone();
        }
    }
}