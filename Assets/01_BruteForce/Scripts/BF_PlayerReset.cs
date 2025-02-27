using UnityEngine;

public class BF_PlayerReset : MonoBehaviour
{
    public GameObject player;
    public Transform playerPos;

    private void OnEnable()
    {
        player.transform.position = playerPos.position;
    }

    private void Start()
    {
        player.transform.position = playerPos.position;
    }

}
