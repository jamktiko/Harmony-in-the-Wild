using UnityEngine;
using UnityEngine.SceneManagement;

//This script will take Vector3 coords from a designated point at DungeonEntrance & QuestPoint
//and will respawn the player at that location after completion.
//TODO: Integrate into saving to act as savepoints
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    [SerializeField] private GameObject player;

    private Vector3 defaultPlayerPosition = new Vector3(1627f, 118f, 360f);
    private Vector3 respawnPosition;
    private bool respawnPositionHasBeenSet = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.Find("Player_Spawn");

        if (player == null)
        {
            Debug.LogWarning("Player GameObject not found in the scene.");
            return;
        }

        string activeSceneName = SceneManager.GetActiveScene().name;
        string overworldSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld);

        if (activeSceneName == overworldSceneName)
        {
            if (respawnPositionHasBeenSet)
            {
                player.transform.position = respawnPosition;
            }
            else
            {
                player.transform.position = defaultPlayerPosition;
            }
        }
        else
        {
            return;
        }
    }

    public void SetRespawnPosition(Vector3 respawnPointPosition)
    {
        respawnPosition = respawnPointPosition;
        respawnPositionHasBeenSet = true;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
