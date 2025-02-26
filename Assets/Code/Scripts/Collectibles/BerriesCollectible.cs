using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Berries : MonoBehaviour
{
    [FormerlySerializedAs("interactable")] [SerializeField] bool _interactable;
    [SerializeField] static int _berryCollectableCount;
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
    private void CollectBerry()
    {
        if (_interactable)
        {
            FoxMovement.Instance.PlayerAnimator.SetBool("isCollectingBerry", true);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(95f, 0.5f)).Append(transform.DOScale(50f, 0.5f)).OnComplete(() =>
            {
                _interactionIndicator.SetActive(false);

                FoxMovement.Instance.PlayerAnimator.SetBool("isCollectingBerry", false);

                PlayerManager.Instance.Berries++;
                if (Steamworks.SteamClient.IsValid)
                {
                    SteamManager.Instance.AchievementProgressBerry("stat_2");
                }
                PlayerManager.Instance.BerryData[transform.parent.name] = false;
                CollectibleNotification(_notifText, "Berries");
                Invoke("CollectibleNotificationDisappear", 7f);
                gameObject.SetActive(false);
            });
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
            CollectBerry();
        }
    }
}
