using UnityEngine;
using UnityEngine.VFX;
public class ChargeJumping : MonoBehaviour, IAbility
{
    public static ChargeJumping Instance;

    public bool IsChargeJumpActivated;
    public bool IsChargingJump;
    [SerializeField] private float _chargeJumpHeight = 24f;
    [SerializeField] private AudioSource _chargeJumpAudio;
    [SerializeField] private AudioSource _chargeJumpLandingAudio;
    [SerializeField] private VisualEffect _chargeJumpVFX;

    private float _chargeJumpTimer;

    private int _onEnableChargeJumpID;
    private int _onDisableChargeJumpID;
    private bool _vfxPlaying;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one ChargeJumping ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _onEnableChargeJumpID = Shader.PropertyToID("OnChargeJumpStart");
        _onDisableChargeJumpID = Shader.PropertyToID("OnChargeJumpStop");
    }
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.ChargeJumping, this);
    }

    public void Activate()
    {
        if (IsChargeJumpActivated && !IsChargingJump)
        {
            IsChargingJump = true;
        }
    }
    private void Update()
    {
        if (IsChargeJumpActivated)
        {
            if (IsChargingJump)
            {
                ChargeJump();
            }

            if (_chargeJumpTimer != 14 && PlayerInputHandler.instance.ChargeJumpInput.WasReleasedThisFrame())
            {
                ReleaseChargedJump();
            }
        }
    }
    private void ChargeJump()
    {
        if (FoxMovement.instance != null)
        {
            if (!_vfxPlaying)
            {
                _chargeJumpVFX.SendEvent(_onEnableChargeJumpID);
                _vfxPlaying = true;
            }


            FoxMovement.instance.rb.velocity = new Vector3(0f, 0f, 0f);

            if (_chargeJumpTimer < _chargeJumpHeight)
            {
                if (!_chargeJumpAudio.isPlaying)
                {
                    _chargeJumpAudio.Play();
                }

                _chargeJumpTimer = _chargeJumpTimer + 0.3f;

                FoxMovement.instance.playerAnimator.SetBool("isChargingJump", true);
                FoxMovement.instance.playerAnimator.SetFloat("horMove", FoxMovement.instance.horizontalInput);
                FoxMovement.instance.playerAnimator.SetFloat("vertMove", FoxMovement.instance.verticalInput);
            }
        }
    }
    private void ReleaseChargedJump()
    {
        if (FoxMovement.instance != null)
        {
            IsChargingJump = false;
            _chargeJumpVFX.SendEvent(_onDisableChargeJumpID);
            _vfxPlaying = false;
            _chargeJumpAudio.Stop();

            FoxMovement.instance.rb.AddForce(transform.up * _chargeJumpTimer, ForceMode.Impulse);

            FoxMovement.instance.playerAnimator.SetBool("isChargingJump", false);
            FoxMovement.instance.playerAnimator.SetBool("isJumping", false);
            Invoke(nameof(ResetChargeJump), 0);
        }
    }
    private void ResetChargeJump()
    {
        _chargeJumpTimer = 14;
        IsChargingJump = false;
    }
}
