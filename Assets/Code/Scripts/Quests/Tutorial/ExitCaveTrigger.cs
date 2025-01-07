using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCaveTrigger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    private void OnTriggerEnter(Collider other)
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex();

        if (other.gameObject.CompareTag("Trigger") && currentQuestStepIndex >= 3)
        {
            ExitCaveQuest.instance.ExitCave();
            GameEventsManager.instance.uiEvents.ShowLoadingScreen("OverWorld - VS");

            //StartCoroutine(LoadSceneWithLoadingScreenWithText2(sceneIndex));
            
        }
    }
    /*IEnumerator LoadSceneWithLoadingScreenWithText2(int sceneId)
    {
        Debug.Log(SaveManager.instance.gameData.playerPositionData);
        GameObject screen = Instantiate(loadingScreen,GameObject.FindGameObjectWithTag("Canvas").transform);
        loadingScreenText = screen.GetComponentInChildren<TMP_Text>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressValue < 0.33f)
            {
                loadingScreenText.text = "Loading.";
                Debug.Log(SaveManager.instance.gameData.playerPositionData);
            }
            else if (progressValue < 0.66f)
            {
                loadingScreenText.text = "Loading..";
                Debug.Log(SaveManager.instance.gameData.playerPositionData);
            }
            else if (progressValue > 0.66f)
            {
                loadingScreenText.text = "Loading...";
                Debug.Log(SaveManager.instance.gameData.playerPositionData);
            }
            yield return null;
        }
    }*/
}
