using UnityEngine;
using UnityEngine.VFX;
public class ChargeJumping : MonoBehaviour, IAbility
{
    public static ChargeJumping Instance;

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
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one ChargeJumping ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        onEnableChargeJumpID = Shader.PropertyToID("OnChargeJumpStart");
        onDisableChargeJumpID = Shader.PropertyToID("OnChargeJumpStop");
    }
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.ChargeJumping, this);
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

            if (chargeJumpTimer != 14 && PlayerInputHandler.Instance.ChargeJumpInput.WasReleasedThisFrame())
            {
                ReleaseChargedJump();
            }
        }
    }
    private void ChargeJump()
    {
        if (FoxMovement.Instance != null)
        {
            if (!vfxPlaying)
            {
                chargeJumpVFX.SendEvent(onEnableChargeJumpID);
                vfxPlaying = true;
            }
            

            FoxMovement.Instance.Rb.velocity = new Vector3(0f, 0f, 0f);

            if (chargeJumpTimer < chargeJumpHeight)
            {
                if (!chargeJumpAudio.isPlaying)
                {
                    chargeJumpAudio.Play();
                }

                chargeJumpTimer = chargeJumpTimer + 0.3f;

                FoxMovement.Instance.playerAnimator.ChargingJump(true);
                FoxMovement.Instance.playerAnimator.HorizontalMove = FoxMovement.Instance.HorizontalInput;
                FoxMovement.Instance.playerAnimator.VerticalMove = FoxMovement.Instance.VerticalInput;
            } 
        }
    }
    private void ReleaseChargedJump()
    {
        if (FoxMovement.Instance != null)
        {
            isChargingJump = false;
            chargeJumpVFX.SendEvent(onDisableChargeJumpID);
            vfxPlaying = false;
            chargeJumpAudio.Stop();

            FoxMovement.Instance.Rb.AddForce(transform.up * chargeJumpTimer, ForceMode.Impulse);
            FoxMovement.Instance.playerAnimator.ChargingJump(false);
            Invoke(nameof(ResetChargeJump), 0); 
        }
    }
    private void ResetChargeJump()
    {
        chargeJumpTimer = 14;
        isChargingJump = false;
    }
}
