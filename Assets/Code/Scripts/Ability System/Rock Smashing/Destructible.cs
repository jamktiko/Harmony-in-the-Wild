using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float averageTimeBetweenSmashableEffects;
    [SerializeField] private bool needsToBeFrozen;
    [SerializeField] private bool isQuestRock;

    [Header("Needed References")]
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private GameObject oreVersion;
    [SerializeField] private GameObject smashableEffect;

    private bool hasOre;
    private bool destroyCurrentObject;
    private bool smashingInProgress;

    private void Start()
    {
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void OnEnable()
    {
        smashingInProgress = false;
    }

    private void SpawnSmashableEffect()
    {
        Instantiate(smashableEffect, transform.parent);
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.RockDestroying, out bool isUnlocked) && isUnlocked && !smashingInProgress)
        {
            if (needsToBeFrozen)
            {
                if (gameObject.GetComponent<Freezable>().isFrozen)
                {
                    AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                    smashingInProgress = true;
                    CreateDestroyedVersion();
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be frozen first.");
                }
            }

            else if (isQuestRock)
            {
                AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                smashingInProgress = true;
                CheckForOre();
            }

            else
            {
                AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                smashingInProgress = true;
                destroyCurrentObject = true;
                CreateDestroyedVersion();
            }
        }
    }

    private void CreateDestroyedVersion()
    {
        if (destroyCurrentObject)
        {
            if (isQuestRock && hasOre)
            {
                Instantiate(oreVersion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else
            {
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        else
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void IncludeOre()
    {
        hasOre = true;
    }

    public void CheckForOre()
    {
        if (hasOre)
        {
            AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
            destroyCurrentObject = true;
            CreateDestroyedVersion();

            SmashingAttemptCounter.instance.UpdateProgress(hasOre);
        }

        else
        {
            AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
            destroyCurrentObject = true;
            CreateDestroyedVersion();

            SmashingAttemptCounter.instance.UpdateProgress(hasOre);
        }
    }

    private IEnumerator DelayBetweenSmashableEffects()
    {
        yield return new WaitForSeconds(Random.Range(averageTimeBetweenSmashableEffects - 2f, averageTimeBetweenSmashableEffects + 2f));

        SpawnSmashableEffect();
    }
}
