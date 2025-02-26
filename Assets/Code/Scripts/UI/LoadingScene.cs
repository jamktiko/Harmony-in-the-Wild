using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    //public GameObject loadingScreen;
    //public Image loadBarFill;
    public TMP_Text loadingScreenText;
    public StorybookHandler storybookHandler;
    //public int sceneIndex;

    private GameObject loadingScreen;

    private void Start()
    {
        try
        {
            storybookHandler = FindObjectOfType<StorybookHandler>();
        }
        catch { }
        ;

        loadingScreen = transform.GetChild(0).gameObject;

        GameEventsManager.instance.uiEvents.OnShowLoadingScreen += ToggleLoadingScene;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.uiEvents.OnShowLoadingScreen -= ToggleLoadingScene;
    }

    private void ToggleLoadingScene(SceneManagerHelper.Scene newScene)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(PlayLoadingScreen(newScene));
    }

    private IEnumerator PlayLoadingScreen(SceneManagerHelper.Scene sceneName)
    {
        AsyncOperation operation;

        AudioManager.Instance.EndCurrentTheme();

        // wait a while, so the audio has time to fade
        yield return new WaitForSeconds(2f);

        if (sceneName == SceneManagerHelper.Scene.NoName)
        {
            operation = SceneManagerHelper.LoadSceneAsync(StorybookHandler.instance.GetNextScene());
        }

        else
        {
            operation = SceneManagerHelper.LoadSceneAsync(sceneName);
        }

        while (!operation.isDone)
        {
            loadingScreenText.text = "Loading.";

            yield return new WaitForSeconds(0.5f);

            loadingScreenText.text = "Loading..";

            yield return new WaitForSeconds(0.5f);

            loadingScreenText.text = "Loading...";

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.2f);

        loadingScreen.SetActive(false);
    }

    //public void LoadSceneWithBar(int sceneId) 
    //{
    //    StartCoroutine(LoadSceneWithLoadingScreenWithBarFill(sceneId));

    //}

    //public void LoadSceneWithText(int sceneId)
    //{
    //    StartCoroutine(LoadSceneWithLoadingScreenWithText(sceneId));
    //}

    //public void LoadSceneWithText2(int sceneId)
    //{
    //    StartCoroutine(LoadSceneWithLoadingScreenWithText2(sceneId));
    //}

    //IEnumerator LoadSceneWithLoadingScreenWithBarFill(int sceneId) 
    //{
    //    AsyncOperation operation=SceneManager.LoadSceneAsync(sceneId);
    //    loadingScreen.SetActive(true);
    //    while (!operation.isDone) 
    //    {
    //        float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
    //        loadBarFill.fillAmount = progressValue;
    //        yield return null;
    //    }
    //}

    //IEnumerator LoadSceneWithLoadingScreenWithText(int sceneId)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(StorybookHandler.instance.GetNextScene());
    //    loadingScreen.SetActive(true);
    //    while (!operation.isDone)
    //    {
    //        float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
    //        if (progressValue<0.33f)
    //        {
    //            loadingScreenText.text = "Loading.";
    //        }
    //        else if (progressValue < 0.66f)
    //        {
    //            loadingScreenText.text = "Loading..";
    //        }
    //        else if (progressValue > 0.66f)
    //        {
    //            loadingScreenText.text = "Loading...";
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator LoadSceneWithLoadingScreenWithText2(int sceneId)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
    //    loadingScreen.SetActive(true);
    //    while (!operation.isDone)
    //    {
    //        float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
    //        if (progressValue < 0.33f)
    //        {
    //            loadingScreenText.text = "Loading.";
    //        }
    //        else if (progressValue < 0.66f)
    //        {
    //            loadingScreenText.text = "Loading..";
    //        }
    //        else if (progressValue > 0.66f)
    //        {
    //            loadingScreenText.text = "Loading...";
    //        }
    //        yield return null;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("lol");
    //    if (other.CompareTag("Trigger"))
    //    {
    //        StartCoroutine(LoadSceneWithLoadingScreenWithText2(sceneIndex));
    //    }
    //}
}
