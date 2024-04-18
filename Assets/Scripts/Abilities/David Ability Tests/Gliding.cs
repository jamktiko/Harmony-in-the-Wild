using UnityEngine;

public class Gliding : MonoBehaviour, IAbility
{
    public static Gliding instance;

    public float glidingMultiplier = 0.4f;
    public float airMultiplier = 0.7f;
    public bool isGliding;
    [SerializeField] private AudioSource glidingAudio;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Gliding ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.Gliding, this);
    }
    private void Update()
    {
        Glide();
        CalculateGlidingMultiplier();

        if (IsGlidingOver())
        {
            DisableGliding();
        }
    }
    public void Activate()
    {
        isGliding = !isGliding;

        Debug.Log("Gliding activated");
    }
    private void Glide()
    {
        if (isGliding)
        {
            if (FoxMovement.instance.rb.useGravity)
            {
                glidingMultiplier = 0.1f;
                FoxMovement.instance.rb.velocity = new Vector3(FoxMovement.instance.rb.velocity.x, 0, FoxMovement.instance.rb.velocity.z);
                FoxMovement.instance.rb.useGravity = false;
                FoxMovement.instance.rb.velocity = new Vector3(0, -1.5f, 0);

                if (!glidingAudio.isPlaying)
                {
                    glidingAudio.Play();
                }
            }

            FoxMovement.instance.rb.velocity = new Vector3(FoxMovement.instance.rb.velocity.x, -1.5f, FoxMovement.instance.rb.velocity.z);

            FoxMovement.instance.playerAnimator.SetBool("isGrounded", false);
            FoxMovement.instance.playerAnimator.SetBool("isGliding", true);
        }
    }

    private bool IsGlidingOver()
    {
        return FoxMovement.instance.IsGrounded() || FoxMovement.instance.IsInWater() || !isGliding;
    }

    private void CalculateGlidingMultiplier()
    {
        if (glidingMultiplier < 0.5)
        {
            glidingMultiplier += 0.005f;
        }
    }

    private void DisableGliding()
    {
        FoxMovement.instance.playerAnimator.SetBool("isGrounded", false);
        isGliding = false;

        if (!FoxMovement.instance.rb.useGravity)
        {
            FoxMovement.instance.rb.useGravity = true;
        }
        FoxMovement.instance.playerAnimator.SetBool("isGliding", false);
    }
}
