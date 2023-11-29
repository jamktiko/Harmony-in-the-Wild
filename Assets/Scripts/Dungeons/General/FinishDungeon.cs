using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeon : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int gainedAbilityIndex;
    [SerializeField] private string goToScene;

    private AudioSource audioSource;
    private string questId;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        questId = questSO.id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ReturnToOverworld());
        }
    }

    private IEnumerator ReturnToOverworld()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);

        GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);

        SceneManager.LoadScene(goToScene);
    }
}
