using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateBear : MonoBehaviour
{
    [SerializeField] private GameObject tutorialBearPrefab;
    [SerializeField] private Vector3 spawnPosition;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SpawnBearIntoScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SpawnBearIntoScene;
    }

    private void SpawnBearIntoScene(Scene newScene, LoadSceneMode mode)
    {
        // NOTE MATCH THIS TO NEW SCENE MANAGEMENT SYSTEM LATER
        if(newScene.name == "Overworld" && !transform.parent.Find("TutorialBear(Clone)"))
        {
            GameObject bear = Instantiate(tutorialBearPrefab, spawnPosition, Quaternion.identity);

            bear.transform.parent = transform.parent;
            bear.transform.Rotate(new Vector3(0, 186, 0));
            bear.gameObject.SetActive(true);
        }
    }
}
