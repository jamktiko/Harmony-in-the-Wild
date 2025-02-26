using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    public static Swimming Instance;
    private bool _isActivated = false;

    public float SwimSpeed = 5f;

    private bool _swimmingAudioPlaying = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one Swimming ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.Swimming, this);
    }

    public void Activate()
    {
        Swim();
        if (!_isActivated)
        {
            Debug.Log("Swimming activated");
            _isActivated = true;
        }
    }

    private void Swim()
    {
        if (FoxMovement.Instance.IsInWater())
        {
            if (!FoxMovement.Instance.Rb.useGravity)
            {
                FoxMovement.Instance.Rb.useGravity = true;
            }

            FoxMovement.Instance.PlayerAnimator.SetFloat("horMove", FoxMovement.Instance.HorizontalInput, 0.1f, Time.deltaTime);
            FoxMovement.Instance.PlayerAnimator.SetFloat("vertMove", FoxMovement.Instance.VerticalInput, 0.1f, Time.deltaTime);
            //FoxMovement.instance.playerAnimator.SetBool("isJumping", false);
            //FoxMovement.instance.playerAnimator.SetBool("isGrounded", true);
            //FoxMovement.instance.playerAnimator.speed = 0.7f;

            if (!_swimmingAudioPlaying)
            {
                _swimmingAudioPlaying = true;
                AudioManager.Instance.PlaySound(AudioName.AbilitySwimming, transform);
            }
        }

        if (FoxMovement.Instance.IsGrounded() && _swimmingAudioPlaying)
        {
            _swimmingAudioPlaying = false;
            GameEventsManager.instance.AudioEvents.DestroyAudio(AudioName.AbilitySwimming);
            _swimmingAudioPlaying = false;
        }
    }
}
