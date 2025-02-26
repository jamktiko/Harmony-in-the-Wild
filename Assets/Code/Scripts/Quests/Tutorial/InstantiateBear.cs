using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InstantiateBear : MonoBehaviour
{
    [FormerlySerializedAs("tutorialBearPrefab")] [SerializeField] private GameObject _tutorialBearPrefab;
    [FormerlySerializedAs("spawnPosition")] [SerializeField] private Vector3 _spawnPosition;
    [FormerlySerializedAs("spawnRotation")] [SerializeField] private Vector3 _spawnRotation = new Vector3(0, 186, 0);

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
        if (newScene.name.Contains("Overworld", System.StringComparison.OrdinalIgnoreCase) && !transform.parent.Find("TutorialBear(Clone)"))
        {
            GameObject bear = Instantiate(_tutorialBearPrefab, _spawnPosition, Quaternion.identity);

            bear.transform.parent = transform.parent;
            bear.transform.Rotate(_spawnRotation);
            bear.gameObject.SetActive(true);
        }
    }
}
