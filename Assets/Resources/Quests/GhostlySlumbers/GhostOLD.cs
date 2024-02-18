using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Ghost Dialogue")]
    [SerializeField] private List<TextAsset> ghostInteractionOptions;

    [Header("Debugging")]
    [SerializeField] private bool playerIsNear;

    private GameObject character;

    private void Start()
    {
        // locate the character object for upcoming visibility toggling
        character = transform.GetChild(0).gameObject;

        StartCoroutine(CheckInitialVisibility());
    }

    private IEnumerator CheckInitialVisibility()
    {
        // wait for a while so that the saved data is loaded before trying to access it
        yield return new WaitForSeconds(0.1f);

        // hide the character if player doesn't have Ghost Speak
        if (!PlayerManager.instance.hasAbilityValues[5])
        {
            character.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnGhostSpeakActivated += SetGhostVisibility;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnGhostSpeakActivated -= SetGhostVisibility;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InteractWithGhost();
        }
    }*/

    private void SetGhostVisibility()
    {
        character.SetActive(true);
    }

    /*public void InteractWithGhost()
    {
        // play random interaction if player is near the ghost and there is at least one dialogue to play
        if (playerIsNear && ghostInteractionOptions.Count != 0)
        {
            DialogueManager.instance.StartDialogue(ghostInteractionOptions[Random.Range(0, ghostInteractionOptions.Count)]);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
