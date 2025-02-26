using TMPro;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    private Vector3 _teleportTarget;
    private Transform _player;

    private void Awake()
    {
    }

    public void InitializeLocation(Vector3 newTarget, string name)
    {
        _teleportTarget = newTarget;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;

        GameObject[] playerTags = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerOption in playerTags)
        {
            if (playerOption.gameObject.name == "Player_Spawn")
            {
                _player = playerOption.transform;
            }
        }
    }

    public void Teleport()
    {
        _player.gameObject.SetActive(false);
        _player.position = _teleportTarget;
        _player.gameObject.SetActive(true);
    }
}
