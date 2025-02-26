using UnityEngine;

public class TeleportUsingMap : MonoBehaviour
{
    private Transform _player;
    private void Start()
    {
        GameObject[] playerTags = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerOption in playerTags)
        {
            if (playerOption.gameObject.name == "Player_Spawn")
            {
                _player = playerOption.transform;
            }
        }
    }
    public void TeleportTo()
    {
        //Debug.Log("Hello yes I been clicked");

        _player.gameObject.SetActive(false);
        _player.transform.position = transform.position;
        _player.gameObject.SetActive(true);
    }
}
