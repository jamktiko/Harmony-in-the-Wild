using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeon : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int gainedAbilityIndex;
    [SerializeField] private int storybookSectionIndex;
    [SerializeField] private string goToScene;

    private AudioSource audioSource;
    private string questId;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(questSO != null)
        {
            questId = questSO.id;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            StartCoroutine(ReturnToOverworld());
        }
    }

    private IEnumerator ReturnToOverworld()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);

        if (questSO != null)
        {
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);
            StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
            SceneManager.LoadScene("Storybook");
        }

        else
        {
            SceneManager.LoadScene(goToScene);
        }
    }
}
