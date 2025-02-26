using TMPro;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    private Vector3 teleportTarget;
    private Transform player;

    private void Awake()
    {
    }

    public void InitializeLocation(Vector3 newTarget, string name)
    {
        teleportTarget = newTarget;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;

        GameObject[] playerTags = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerOption in playerTags)
        {
            if (playerOption.gameObject.name == "Player_Spawn")
            {
                player = playerOption.transform;
            }
        }
    }

    public void Teleport()
    {
        player.gameObject.SetActive(false);
        player.position = teleportTarget;
        player.gameObject.SetActive(true);
    }
}
