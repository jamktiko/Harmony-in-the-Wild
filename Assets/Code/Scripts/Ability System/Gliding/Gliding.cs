using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour, IAbility
{
    public static Gliding Instance;

    public float GlidingMultiplier = 0.4f;
    public float AirMultiplier = 0.7f;
    public bool IsGliding;
    [SerializeField] private AudioSource _glidingAudio;
    private readonly List<ParticleSystem> _glideParticleEmission = new List<ParticleSystem>();

    private bool _allowActivationBasedOnInput = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one Gliding ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleActivation;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleActivation;
    }

    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.Gliding, this);
    }
    private void Update()
    {
        Glide();
        CalculateGlidingMultiplier();

        if (IsGlidingOver() && FoxMovement.Instance != null)
        {
            DisableGliding();
        }
        if (_glideParticleEmission.Count < 1 || _glideParticleEmission[0] == null)
        {
            _glideParticleEmission.Clear();
            Transform player = null;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                player = obj.transform;
                break;
            }
            if (player)
            {
                while (player.parent != null)
                    player = player.parent;
                FindVFX(player);
            }
        }
    }

    private void FindVFX(Transform cur)
    {
        if (cur.gameObject.name == "GlideTrail" || cur.gameObject.name == "GlideParticles")
        {
            ParticleSystem curParticles = cur.gameObject.GetComponent<ParticleSystem>();
            if (curParticles != null)
            {
                _glideParticleEmission.Add(curParticles);
                var emission = curParticles.emission;
                emission.enabled = false;
            }
        }
        foreach (Transform t in cur)
            FindVFX(t);
    }

    public void Activate()
    {
        if (_allowActivationBasedOnInput)
        {
            IsGliding = !IsGliding;

            Debug.Log("Gliding activated");

            if (IsGliding)
            {
                AudioManager.Instance.PlaySound(AudioName.AbilityGlidingActivated, transform);

                for (int i = 0; i < _glideParticleEmission.Count; i++)
                {
                    var emission = _glideParticleEmission[i].emission;
                    emission.enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < _glideParticleEmission.Count; i++)
                {
                    var emission = _glideParticleEmission[i].emission;
                    emission.enabled = false;
                }
            }
        }
    }
    private void Glide()
    {
        if (IsGliding)
        {
            if (FoxMovement.Instance.Rb.useGravity)
            {
                GlidingMultiplier = 0.1f;
                FoxMovement.Instance.Rb.velocity = new Vector3(FoxMovement.Instance.Rb.velocity.x, 0, FoxMovement.Instance.Rb.velocity.z);
                FoxMovement.Instance.Rb.useGravity = false;
                FoxMovement.Instance.Rb.velocity = new Vector3(0, -1.5f, 0);

                //if (!glidingAudio.isPlaying)
                //{
                //    glidingAudio.Play();
                //}
            }

            FoxMovement.Instance.Rb.velocity = new Vector3(FoxMovement.Instance.Rb.velocity.x, -1.5f, FoxMovement.Instance.Rb.velocity.z);

            FoxMovement.Instance.playerAnimator.IsGrounded = false;
            FoxMovement.Instance.playerAnimator.IsGliding = true;
        }
    }

    private bool IsGlidingOver()
    {
        if (FoxMovement.Instance != null)
        {
            return FoxMovement.Instance.IsGrounded() || FoxMovement.Instance.IsInWater() || !IsGliding;
        }
        return true;
    }

    private void CalculateGlidingMultiplier()
    {
        if (GlidingMultiplier < 0.5)
        {
            GlidingMultiplier += 0.005f;
        }
    }

    private void DisableGliding()
    {
        FoxMovement.Instance.playerAnimator.IsGrounded = false;
        IsGliding = false;

        if (!FoxMovement.Instance.Rb.useGravity)
        {
            FoxMovement.Instance.Rb.useGravity = true;
        }
        FoxMovement.Instance.playerAnimator.IsGliding = false;

        if (_glideParticleEmission != null)
        {
            for (int i = 0; i < _glideParticleEmission.Count; i++)
            {
                if (_glideParticleEmission[i] = null)
                {
                    var emission = _glideParticleEmission[i].emission;
                    emission.enabled = false;
                }
            }
        }
    }

    private void ToggleActivation(bool enabled)
    {
        _allowActivationBasedOnInput = enabled;
    }
}
