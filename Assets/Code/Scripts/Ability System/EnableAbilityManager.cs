using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableAbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject abilityManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += EnableManager;
    }

    private void EnableManager(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Tutorial")
        {
            abilityManager.SetActive(true);
            Destroy(this);
        }
    }
}