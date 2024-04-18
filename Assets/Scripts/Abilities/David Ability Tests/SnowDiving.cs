using UnityEngine;
using UnityEngine.VFX;

public class SnowDiving : MonoBehaviour, IAbility
{
    public static SnowDiving instance;

    public float snowDiveSpeed = 15f;
    [SerializeField] private AudioSource snowDivingAudio;

    [SerializeField] private VisualEffect snowDiveVFX;
    private int onEnableSnowDiveID;

    [SerializeField] private float snowDiveTimer = 3f;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one SnowDiving ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        onEnableSnowDiveID = Shader.PropertyToID("onSnowDive");
    }
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.SnowDiving, this);
    }

    public void Activate()
    {
        SnowDive();

        if (Physics.Raycast(FoxMovement.instance.cameraPosition.position, FoxMovement.instance.cameraPosition.forward, out RaycastHit hit, 50f, FoxMovement.instance.climbWallLayerMask))
        {
            ClimbSnowWall(hit);
        }
    }
    private void SnowDive()
    {
        if (FoxMovement.instance.IsGrounded())
        {
            //snowDiveVFX.SendEvent(onEnableSnowDiveID);

            FoxMovement.instance.playerAnimator.SetBool("isGliding", false);
        }
    }

    private void ClimbSnowWall(RaycastHit hit)
    {
        //climbing animation here (later will make more code for this)
        snowDiveVFX.SendEvent(onEnableSnowDiveID);
        gameObject.transform.position = hit.transform.GetChild(0).position;
    }
}
