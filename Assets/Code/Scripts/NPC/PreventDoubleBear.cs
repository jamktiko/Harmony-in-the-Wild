using UnityEngine;
using UnityEngine.SceneManagement;

public class PreventDoubleBear : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;

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
            QuestState currentState = QuestManager.instance.CheckQuestState(tutorialQuest.id);

            if (currentState == QuestState.FINISHED)
            {
                Destroy(gameObject);
            }
        }
    }
}
