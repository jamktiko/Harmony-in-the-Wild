using System.Collections;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportationTarget;
    [SerializeField] private int stageIndex;
    [SerializeField] private string questName;

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
        FoxMovement.instance.gameObject.SetActive(false);
        player.position = teleportationTarget.position;
        FoxMovement.instance.gameObject.SetActive(true);
        Debug.Log("teleporting player");
        GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questName, stageIndex);
    }
}
