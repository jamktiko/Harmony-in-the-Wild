using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableAbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject _abilityManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += EnableManager;
    }

    private void EnableManager(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tutorial")
        {
            _abilityManager.SetActive(true);
            Destroy(this);
        }
    }
}