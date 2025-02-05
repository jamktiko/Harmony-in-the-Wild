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

    private void Start()
    {
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void SpawnSmashableEffect()
    {
        Instantiate(smashableEffect, transform.parent);
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.RockDestroying, out bool isUnlocked) && isUnlocked)
        {
            if (needsToBeFrozen)
            {
                if (gameObject.GetComponent<Freezable>().isFrozen)
                {
                    AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                    Instantiate(destroyedVersion, transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be frozen first.");
                }
            }

            else if (isQuestRock)
            {
                CheckForOre();
            }

            else
            {
                AudioManager.instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
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
            SmashingAttemptCounter.instance.UpdateProgress(hasOre);
            Instantiate(oreVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        else
        {
            SmashingAttemptCounter.instance.UpdateProgress(hasOre);
            Instantiate(destroyedVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayBetweenSmashableEffects()
    {
        yield return new WaitForSeconds(Random.Range(averageTimeBetweenSmashableEffects - 2f, averageTimeBetweenSmashableEffects + 2f));

        SpawnSmashableEffect();
    }
}
