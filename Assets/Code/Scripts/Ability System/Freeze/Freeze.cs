using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Freeze : MonoBehaviour
{
    public const string DungeonPenguinSceneName = "Dungeon_Penguin";

    [Header("Config")]
    [SerializeField] private float _aoeRadius;
    [SerializeField] private float _cooldownDuration;

    [SerializeField] private Image _coloredCooldownIndicator;

    [Header("Audio")]
    [SerializeField] private AudioSource _freezeAudio;

    private bool _hasCooldown;

    private void Update()
    {
        if (PlayerInputHandler.instance.UseAbilityInput.WasPressedThisFrame())
        {
            AbilityManager.Instance._abilityStatuses.TryGetValue(Abilities.Freezing, out bool isUnlocked);

            if (isUnlocked && !_hasCooldown)
            {
                ActivateFreezeObject();
            }
        }
    }

    private void ActivateFreezeObject()
    {

        Collider[] foundObjects = Physics.OverlapSphere(transform.position, _aoeRadius, LayerMask.GetMask("Freezables"));
        Debug.Log(foundObjects.Length + " freezables found.");

        if (foundObjects != null)
        {
            foreach (Collider newObject in foundObjects)
            {
                FoxMovement.instance.playerAnimator.SetBool("isFreezing", true);
                Freezable freezable = newObject.gameObject.GetComponent<Freezable>();

                if (freezable)
                {
                    freezable.FreezeObject();

                    AudioManager.Instance.PlaySound(AudioName.Ability_Freezing, transform);
                }
            }

        }

        if (SceneManager.GetActiveScene().name == DungeonPenguinSceneName)
        {
            StartCoroutine(FreezeCooldown());
        }
    }

    private IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        FoxMovement.instance.playerAnimator.SetBool("isFreezing", false);
        _hasCooldown = true;

        float updateFillAmount = 1 / (_cooldownDuration * 100);
        _coloredCooldownIndicator.fillAmount = 0;

        while (_coloredCooldownIndicator.fillAmount != 1)
        {
            _coloredCooldownIndicator.fillAmount += updateFillAmount;

            yield return new WaitForSeconds(0.01f);
        }

        _hasCooldown = false;
    }
}
