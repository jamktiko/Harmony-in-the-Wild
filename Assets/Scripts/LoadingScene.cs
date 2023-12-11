using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadBarFill;
    public TMP_Text loadingScreenText;

    public void loadSceneWithBar(int sceneId) 
    {
        StartCoroutine(loadSceneWithLoadingScreenWithBarFill(sceneId));
        
    }
    public void loadSceneWithText(int sceneId)
    {
        StartCoroutine(loadSceneWithLoadingScreenWithText(sceneId));
    }

     IEnumerator loadSceneWithLoadingScreenWithBarFill(int sceneId) 
    {
        AsyncOperation operation=SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!operation.isDone) 
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
    IEnumerator loadSceneWithLoadingScreenWithText(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressValue<0.33f)
            {
                loadingScreenText.text = "Loading.";
            }
            else if (progressValue < 0.66f)
            {
                loadingScreenText.text = "Loading..";
            }
            else if (progressValue > 0.66f)
            {
                loadingScreenText.text = "Loading...";
            }
            yield return null;
        }
    }
}
