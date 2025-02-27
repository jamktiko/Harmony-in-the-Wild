using UnityEngine;
using UnityEngine.VFX;
public class ChargeJumping : MonoBehaviour, IAbility
{
    public static ChargeJumping instance;

    public bool isChargeJumpActivated;
    public bool isChargingJump;
    [SerializeField] private float chargeJumpHeight = 24f;
    [SerializeField] private AudioSource chargeJumpAudio;
    [SerializeField] private AudioSource chargeJumpLandingAudio;
    [SerializeField] private VisualEffect chargeJumpVFX;

    private float chargeJumpTimer;

    private int onEnableChargeJumpID;
    private int onDisableChargeJumpID;
    private bool vfxPlaying;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one ChargeJumping ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        onEnableChargeJumpID = Shader.PropertyToID("OnChargeJumpStart");
        onDisableChargeJumpID = Shader.PropertyToID("OnChargeJumpStop");
    }
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.ChargeJumping, this);
    }

    public void Activate()
    {
        if (isChargeJumpActivated && !isChargingJump)
        {
            isChargingJump = true;
        }
    }
    private void Update()
    {
        if (isChargeJumpActivated)
        {
            if (isChargingJump)
            {
                ChargeJump();
            }

            if (chargeJumpTimer != 14 && PlayerInputHandler.instance.ChargeJumpInput.WasReleasedThisFrame())
            {
                ReleaseChargedJump();
            }
        }
    }
    private void ChargeJump()
    {
        if (FoxMovement.instance != null)
        {
            if (!vfxPlaying)
            {
                chargeJumpVFX.SendEvent(onEnableChargeJumpID);
                vfxPlaying = true;
            }
            

            FoxMovement.instance.rb.velocity = new Vector3(0f, 0f, 0f);

            if (chargeJumpTimer < chargeJumpHeight)
            {
                if (!chargeJumpAudio.isPlaying)
                {
                    chargeJumpAudio.Play();
                }

                chargeJumpTimer = chargeJumpTimer + 0.3f;

                FoxMovement.instance.playerAnimator.ChargingJump(true);
                FoxMovement.instance.playerAnimator.HorizontalMove = FoxMovement.instance.horizontalInput;
                FoxMovement.instance.playerAnimator.VerticalMove = FoxMovement.instance.verticalInput;
            } 
        }
    }
    private void ReleaseChargedJump()
    {
        if (FoxMovement.instance != null)
        {
            isChargingJump = false;
            chargeJumpVFX.SendEvent(onDisableChargeJumpID);
            vfxPlaying = false;
            chargeJumpAudio.Stop();

            FoxMovement.instance.rb.AddForce(transform.up * chargeJumpTimer, ForceMode.Impulse);
            FoxMovement.instance.playerAnimator.ChargingJump(false);
            Invoke(nameof(ResetChargeJump), 0); 
        }
    }
    private void ResetChargeJump()
    {
        chargeJumpTimer = 14;
        isChargingJump = false;
    }
}
