using UnityEngine;
using UnityEngine.SceneManagement;

public class ChargeJumping : MonoBehaviour, IAbility
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource chargeJumpAudio;

    public float ChargeJumpHeight = 22f;
    private float chargeJumpTimer = 14;
    private bool isChargeJumping;

    private void Awake()
    {
        Debug.Log("ChargeJumping Awake");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log("ChargeJumping Start");

        AbilityManager.instance.RegisterAbility(Abilities.ChargeJumping, this);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneManager OnSceneLoaded worked");

        rb = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        Debug.Log("ChargeJumping.Activate() called");
        if (!isChargeJumping)
        {
            Debug.Log("ChargeJumping activated");
            isChargeJumping = true;
            ChargeJump();
        }
    }

    private void ChargeJump()
    {
        Debug.Log("ChargeJump method called");

        rb.velocity = new Vector3(0f, 0f, 0f);

        if (chargeJumpTimer < ChargeJumpHeight)
        {
            //audio play
            if (!chargeJumpAudio.isPlaying)
            {
                chargeJumpAudio.Play();
            }

            chargeJumpTimer = chargeJumpTimer + 0.3f;
        }
    }

    public void Deactivate()
    {
        Debug.Log("ChargeJumping deactivated");
    }
}
