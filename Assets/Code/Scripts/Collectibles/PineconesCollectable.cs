using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PineconesCollectable : MonoBehaviour
{
    [FormerlySerializedAs("interactable")] [SerializeField]
    private bool _interactable;
    [SerializeField] private static int _pineCollectableCount;
    [FormerlySerializedAs("interactionIndicator")] [SerializeField] private GameObject _interactionIndicator;
    [FormerlySerializedAs("notifText")] [SerializeField] private TMP_Text _notifText;

    private bool _hasBeenCollected = false;

    private void Start()
    {
        _notifText = GameObject.Find("CollectibleNotification").GetComponent<TMP_Text>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            _interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            _interactable = false;
        }
    }
    private void CollectCone()
    {
        if (_interactable)
        {
            FoxMovement.Instance.playerAnimator.CollectFromGround();
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(1.3f, 1.3f)).Append(transform.DOScale(0f, 1.3f)).OnComplete(() =>
            {
                _interactionIndicator.SetActive(false);

                // TODO: Review this code; using trigger instead of bool
                // Collecting is now a trigger, no need to reset
                //FoxMovement.instance.playerAnimator.SetBool(FoxAnimation.Parameter.isCollectingPinecone,false);

                PlayerManager.Instance.PineCones++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.Instance.AchievementProgressPinecone("stat_3");
                }
                PlayerManager.Instance.PineConeData[transform.name] = false;
                CollectibleNotification(_notifText, "Pinecones");
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
    private void CollectibleNotification(TMP_Text notifText, string collectibleType)
    {
        notifText.text = collectibleType + " collected: " + (collectibleType.Contains("Berries") ? PlayerManager.Instance.Berries : PlayerManager.Instance.PineCones);
    }
    private void CollectibleNotificationDisappear()
    {
        _notifText.text = "";
    }
    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPerformedThisFrame() && _interactable && !_hasBeenCollected)
        {
            _hasBeenCollected = true;

            CollectCone();
        }
    }
}