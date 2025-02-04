using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    public static Swimming instance;
    private bool isActivated = false;

    public float swimSpeed = 5f;

    private bool swimmingAudioPlaying = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Swimming ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.Swimming, this);
    }

    public void Activate()
    {
        Swim();
        if (!isActivated) 
        { 
            Debug.Log("Swimming activated");
            isActivated = true;
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

            FoxMovement.instance.playerAnimator.SetFloat("horMove", FoxMovement.instance.horizontalInput,0.1f,Time.deltaTime);
            FoxMovement.instance.playerAnimator.SetFloat("vertMove", FoxMovement.instance.verticalInput,0.1f,Time.deltaTime);
            //FoxMovement.instance.playerAnimator.SetBool("isJumping", false);
            //FoxMovement.instance.playerAnimator.SetBool("isGrounded", true);
            //FoxMovement.instance.playerAnimator.speed = 0.7f;

            if (!swimmingAudioPlaying)
            {
                swimmingAudioPlaying = true;
                AudioManager.instance.PlaySound(AudioName.Ability_Swimming, transform);
            }
        }

        if (FoxMovement.instance.IsGrounded() && swimmingAudioPlaying)
        {
            GameEventsManager.instance.audioEvents.DestroyAudio(AudioName.Ability_Swimming);
        }
    }
}
