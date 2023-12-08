using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportationTarget;

    private AudioSource audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        //audioSource.Play();

        //yield return new WaitForSeconds(audioSource.clip.length * 0.75f);
        yield return new WaitForSeconds(0.75f);
        player.position = teleportationTarget.position;
    }
}
