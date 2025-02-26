using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PreventDoubleBear : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuest")] [SerializeField] private QuestScriptableObject _tutorialQuest;

    private void Awake()
    {
        SceneManager.sceneLoaded += DeleteExtraBear;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DeleteExtraBear;
    }

    private void DeleteExtraBear(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            QuestState currentState = QuestManager.Instance.CheckQuestState(_tutorialQuest.id);

            if (currentState == QuestState.Finished)
            {
                Destroy(gameObject);
            }
        }
    }
}
