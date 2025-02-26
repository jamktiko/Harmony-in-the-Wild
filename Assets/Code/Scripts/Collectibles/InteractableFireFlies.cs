using UnityEngine;
using UnityEngine.Serialization;

public class InteractableFireFlies : MonoBehaviour
{
    [FormerlySerializedAs("interactable")] [SerializeField] bool _interactable;
    [SerializeField] static int _pineCollectableCount;
    [FormerlySerializedAs("interactionIndicator")] [SerializeField] private GameObject _interactionIndicator;
    [FormerlySerializedAs("PlayerAnimator")] [SerializeField] private Animator _playerAnimator;

    private bool _hasBeenCollected = false;

    private void Start()
    {
        _playerAnimator = FoxMovement.Instance.gameObject.GetComponentInChildren<Animator>();
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
    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPerformedThisFrame() && _interactable)
        {

            PlayAnimation();
        }
    }
    private void PlayAnimation()
    {
        _playerAnimator.Play("PL_Playful2_ANI");

    }
}
