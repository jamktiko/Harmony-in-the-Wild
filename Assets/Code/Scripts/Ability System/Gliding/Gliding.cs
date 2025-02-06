using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour, IAbility
{
    public static Gliding instance;

    public float glidingMultiplier = 0.4f;
    public float airMultiplier = 0.7f;
    public bool isGliding;
    [SerializeField] private AudioSource glidingAudio;
    private List<ParticleSystem> glideParticleEmission = new List<ParticleSystem>();

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

        if (IsGlidingOver() && FoxMovement.instance != null)
        {
            DisableGliding();
        }
        if (glideParticleEmission.Count < 1 || glideParticleEmission[0] == null)
        {
            glideParticleEmission.Clear();
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
                glideParticleEmission.Add(curParticles);
                var emission = curParticles.emission;
                emission.enabled = false;
            }
        }
        foreach (Transform t in cur)
            FindVFX(t);
    }

    public void Activate()
    {
        isGliding = !isGliding;

        Debug.Log("Gliding activated");

        if (isGliding)
        {
            AudioManager.instance.PlaySound(AudioName.Ability_Gliding_Activated, transform);

            for (int i = 0; i < glideParticleEmission.Count; i++)
            {
                var emission = glideParticleEmission[i].emission;
                emission.enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < glideParticleEmission.Count; i++)
            {
                var emission = glideParticleEmission[i].emission;
                emission.enabled = false;
            }
        }
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

                //if (!glidingAudio.isPlaying)
                //{
                //    glidingAudio.Play();
                //}
            }

            FoxMovement.instance.rb.velocity = new Vector3(FoxMovement.instance.rb.velocity.x, -1.5f, FoxMovement.instance.rb.velocity.z);

            FoxMovement.instance.playerAnimator.SetBool("isGrounded", false);
            FoxMovement.instance.playerAnimator.SetBool("isGliding", true);
        }
    }

    private bool IsGlidingOver()
    {
        if (FoxMovement.instance != null)
        {
            return FoxMovement.instance.IsGrounded() || FoxMovement.instance.IsInWater() || !isGliding;
        }
        return true;
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

        if (glideParticleEmission != null)
        {
            for (int i = 0; i < glideParticleEmission.Count; i++)
            {
                if(glideParticleEmission[i] = null)
                {
                    var emission = glideParticleEmission[i].emission;
                    emission.enabled = false;
                }          
            }
        }
    }
}
