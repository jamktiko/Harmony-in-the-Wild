using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    public static Swimming Instance;
    private bool _isActivated = false;

    public float _swimSpeed = 5f;

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
        if (FoxMovement.instance.IsInWater())
        {
            if (!FoxMovement.instance.rb.useGravity)
            {
                FoxMovement.instance.rb.useGravity = true;
            }

            FoxMovement.instance.playerAnimator.SetFloat("horMove", FoxMovement.instance.horizontalInput, 0.1f, Time.deltaTime);
            FoxMovement.instance.playerAnimator.SetFloat("vertMove", FoxMovement.instance.verticalInput, 0.1f, Time.deltaTime);
            //FoxMovement.instance.playerAnimator.SetBool("isJumping", false);
            //FoxMovement.instance.playerAnimator.SetBool("isGrounded", true);
            //FoxMovement.instance.playerAnimator.speed = 0.7f;

            if (!swimmingAudioPlaying)
            {
                swimmingAudioPlaying = true;
                AudioManager.Instance.PlaySound(AudioName.Ability_Swimming, transform);
            }
        }

        if (FoxMovement.instance.IsGrounded() && swimmingAudioPlaying)
        {
            swimmingAudioPlaying = false;
            GameEventsManager.instance.audioEvents.DestroyAudio(AudioName.Ability_Swimming);
            swimmingAudioPlaying = false;
        }
    }
}
