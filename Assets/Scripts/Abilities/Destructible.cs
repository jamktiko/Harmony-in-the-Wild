using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float averageTimeBetweenSmashableEffects;
    [SerializeField] private bool needsToBeFreezed;
    [SerializeField] private bool isQuestRock;

    [Header("Needed References")]
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private GameObject oreVersion;
    [SerializeField] private GameObject smashableEffect;

    [SerializeField] private AudioSource audioSource;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && PlayerManager.instance.abilityValues[4])
        {
            if (needsToBeFreezed)
            {
                if (gameObject.GetComponent<Freezable>().isFreezed)
                {
                    //audioSource.Play();
                    Instantiate(destroyedVersion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be freezed first.");
                }
            }

            else if (isQuestRock)
            {
                CheckForOre();
            }

            else
            {
                //audioSource.Play();
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
