using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayQuestCompletedSound : MonoBehaviour
{
    [SerializeField] private GameObject audioPrefab;

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest += PlayQuestCompletedAudio;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest -= PlayQuestCompletedAudio;
    }

    private void PlayQuestCompletedAudio(string id)
    {
        if(audioPrefab != null)
        {
            Instantiate(audioPrefab, transform);
        }
    }
}
