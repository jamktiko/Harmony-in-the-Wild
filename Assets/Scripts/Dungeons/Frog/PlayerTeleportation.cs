using System.Collections;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportationTarget;
    [SerializeField] private int newStageIndex;

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
        yield return new WaitForSeconds(0.2f);
        player.position = teleportationTarget.position;

        GameEventsManager.instance.questEvents.AdvanceQuest("The Leaping Frog");
    }
}
