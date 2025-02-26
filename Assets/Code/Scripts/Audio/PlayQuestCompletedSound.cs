using UnityEngine;
using UnityEngine.Serialization;

public class PlayQuestCompletedSound : MonoBehaviour
{
    [FormerlySerializedAs("audioPrefab")] [SerializeField] private GameObject _audioPrefab;

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest += PlayQuestCompletedAudio;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= PlayQuestCompletedAudio;
    }

    private void PlayQuestCompletedAudio(string id)
    {
        if (_audioPrefab != null)
        {
            Instantiate(_audioPrefab, transform);
        }
    }
}
