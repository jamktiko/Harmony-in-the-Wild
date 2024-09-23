using UnityEngine;
using UnityEngine.SceneManagement;

//This script will take Vector3 coords from a designated point at DungeonEntrance & QuestPoint
//and will respawn the player at that location after completion.
//TODO: Integrate into saving to act as savepoints
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    private Vector3 defaultStartingPosition = new Vector3(1627f, 118f, 360f);
    private Vector3 checkpointPosition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Respawn Manager.");
            Destroy(gameObject);
            return;
        }
        else 
        {
            instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld))
        {
            FoxMovement.instance.transform.position = checkpointPosition != null ? checkpointPosition : defaultStartingPosition;
        }
    }

    public void SetRespawnPosition(Vector3 respawnPointPosition)
    {
        checkpointPosition = respawnPointPosition;
        SaveManager.instance.SaveGame();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
