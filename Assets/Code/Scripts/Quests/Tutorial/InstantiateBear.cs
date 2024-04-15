using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateBear : MonoBehaviour
{
    [SerializeField] private GameObject bearPrefab;
    [SerializeField] private Vector3 spawnPosition;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SpawnBearIntoScene;
    }

    private void SpawnBearIntoScene(Scene newScene, LoadSceneMode mode)
    {
        // NOTE MATCH THIS TO NEW SCENE MANAGEMENT SYSTEM LATER
        if(newScene.name == "Overworld")
        {
            GameObject tutorialBear = Instantiate(bearPrefab, spawnPosition, Quaternion.identity);
            tutorialBear.transform.parent = transform.parent;
            tutorialBear.transform.Rotate(new Vector3(0, 186, 0));
        }
    }
}
