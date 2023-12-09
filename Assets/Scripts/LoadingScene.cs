using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadBarFill;

    public void loadScene(int sceneId) 
    {
        StartCoroutine(loadSceneWithLoadingScreen(sceneId));
    }

    IEnumerator loadSceneWithLoadingScreen(int sceneId) 
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
}
