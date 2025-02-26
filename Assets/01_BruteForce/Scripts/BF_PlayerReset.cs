using UnityEngine;

public class BF_PlayerReset : MonoBehaviour
{
    public GameObject player;
    public Transform playerPos;

    void OnEnable()
    {
        player.transform.position = playerPos.position;
    }
    void Start()
    {
        player.transform.position = playerPos.position;
    }

}
