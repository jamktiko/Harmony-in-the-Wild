using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    public static Swimming instance;
    private bool isActivated = false;

    public float swimSpeed = 5f;
    [SerializeField] private AudioSource swimmingAudio;
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

            FoxMovement.instance.playerAnimator.SetFloat("horMove", 1);
            FoxMovement.instance.playerAnimator.SetFloat("vertMove", 0);
            FoxMovement.instance.playerAnimator.SetBool("isJumping", false);
            FoxMovement.instance.playerAnimator.SetBool("isGrounded", true);
            FoxMovement.instance.playerAnimator.speed = 0.7f;

            if (!swimmingAudio.isPlaying)
            {
                swimmingAudio.Play();
            }
        }

        if (FoxMovement.instance.IsGrounded() && swimmingAudio.isPlaying)
        {
            swimmingAudio.Stop();
        }
    }
}
