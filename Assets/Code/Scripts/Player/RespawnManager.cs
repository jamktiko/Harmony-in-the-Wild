using UnityEngine;

//This script will take Vector3 coords from a designated point at DungeonEntrance & QuestPoint
//and will respawn the player at that location after completion.
//TODO: Integrate into saving to act as savepoints
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private Vector3 _defaultStartingPosition = new Vector3(219f, 103f, 757f);
    private Vector3 _checkpointPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one Respawn Manager.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void SetRespawnPosition(Vector3 respawnPointPosition)
    {
        _checkpointPosition = respawnPointPosition;
        SaveManager.Instance.SaveGame();
    }

    public PositionData GetLatestRespawnPoint()
    {
        PositionData respawnPoint = new PositionData();

        if (_checkpointPosition == Vector3.zero)
        {
            respawnPoint.X = _defaultStartingPosition.x;
            respawnPoint.Y = _defaultStartingPosition.y;
            respawnPoint.Z = _defaultStartingPosition.z;

            Debug.Log("Using default values for position saving.");
        }

        else
        {
            // set new position
            respawnPoint.X = _checkpointPosition.x;
            respawnPoint.Y = _checkpointPosition.y;
            respawnPoint.Z = _checkpointPosition.z;

            Debug.Log("Using custom values for position saving.");
        }

        return respawnPoint;
    }
}
