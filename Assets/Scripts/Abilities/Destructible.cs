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

    private bool hasOre;

    private void Update()
    {
        // NOTE DEBUGGING ONLY FOR NOW
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.S))
        {
            PlayerManager.instance.abilityValues[4] = true;
            Debug.Log("Player can rock smash now.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerManager.instance.abilityValues[4])
        {
            if (needsToBeFreezed)
            {
                if (gameObject.GetComponent<Freezable>().isFreezed)
                {
                    Debug.Log("Using rock smash on " + gameObject.name);
                    Instantiate(destroyedVersion, transform.position, Quaternion.identity);
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
                Instantiate(destroyedVersion, transform.position, Quaternion.identity);
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
        Debug.Log("checking");
        if (hasOre)
        {
            SmashingManager.instance.UpdateProgress(hasOre);
            Instantiate(oreVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        else
        {
            SmashingManager.instance.UpdateProgress(hasOre);
            Instantiate(destroyedVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
