using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool needsToBeFreezed;
    [SerializeField] private bool isQuestRock;

    [Header("Needed References")]
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private GameObject oreVersion;

    [SerializeField] private AudioSource audioSource;

    private bool hasOre;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerManager.instance.abilityValues[4])
        {
            if (needsToBeFreezed)
            {
                if (gameObject.GetComponent<Freezable>().isFreezed)
                {
                    audioSource.Play();
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
        Debug.Log("checking for ore");
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
}
