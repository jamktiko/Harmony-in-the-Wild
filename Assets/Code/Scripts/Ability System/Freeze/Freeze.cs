using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Freeze : MonoBehaviour
{
    public const string DungeonPenguinSceneName = "Dungeon_Penguin";

    [Header("Config")]
    [SerializeField] private float aoeRadius;
    [SerializeField] private float cooldownDuration;

    [SerializeField] private Image coloredCooldownIndicator;

    [Header("Audio")]
    [SerializeField] AudioSource freezeAudio;

    private bool hasCooldown;

    private void Update()
    {
        if (PlayerInputHandler.instance.UseAbilityInput.WasPressedThisFrame())
        {
            AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.Freezing, out bool isUnlocked);

            if(isUnlocked && !hasCooldown)
            {
                ActivateFreezeObject();
            }
        }
    }

    private void ActivateFreezeObject()
    {
        
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, aoeRadius, LayerMask.GetMask("Freezables"));
        Debug.Log(foundObjects.Length + " freezables found.");

        if(foundObjects != null)
        {
            foreach(Collider newObject in foundObjects)
            {
                FoxMovement.instance.playerAnimator.SetBool(FoxAnimation.Parameter.isFreezing, true);
                Freezable freezable = newObject.gameObject.GetComponent<Freezable>();

                if (freezable)
                {
                    freezable.FreezeObject();

                    AudioManager.instance.PlaySound(AudioName.Ability_Freezing, transform);
                }
            }
            
        }

        if(SceneManager.GetActiveScene().name == DungeonPenguinSceneName)
        {
            StartCoroutine(FreezeCooldown());
        }
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        FoxMovement.instance.playerAnimator.SetBool(FoxAnimation.Parameter.isFreezing, false);
        hasCooldown = true;

        float updateFillAmount = 1 / (cooldownDuration * 100);
        coloredCooldownIndicator.fillAmount = 0;
        
        while(coloredCooldownIndicator.fillAmount != 1)
        {
            coloredCooldownIndicator.fillAmount += updateFillAmount;

            yield return new WaitForSeconds(0.01f);
        }

        hasCooldown = false;
    }
}
