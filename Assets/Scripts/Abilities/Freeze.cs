using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float aoeRadius;

    [Header("Audio")]
    [SerializeField] AudioSource FreezeAudio;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerManager.instance.abilityValues[7])
        {
            ActivateFreeze();
        }

        // NOTE FOR TESTING ONLY, REMOVE LATER
        if(Input.GetKey(KeyCode.F) && Input.GetKeyDown(KeyCode.R))
        {
            PlayerManager.instance.getAbility(7);
            Debug.Log("Freeze has been unlocked.");
        }
    }

    private void ActivateFreeze()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, aoeRadius, LayerMask.GetMask("Freezables"));
        Debug.Log(foundObjects.Length + " freezables found.");

        if(foundObjects != null)
        {
            foreach(Collider newObject in foundObjects)
            {
                Freezable freezable = newObject.gameObject.GetComponent<Freezable>();

                if (freezable)
                {
                    freezable.Freeze();
                    FreezeAudio.Play();
                }
            }
        }

        Debug.Log("No colliders detected for freezing.");
    }
}
