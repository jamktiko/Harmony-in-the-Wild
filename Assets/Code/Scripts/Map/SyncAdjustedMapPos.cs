using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncAdjustedMapPos : MonoBehaviour
{
    public Image playerImage;
    public Image mapImage;
    public Vector2 mapSize;
    public Vector2 worldSize;
    public Vector2 offset;
    public RectTransform mapAnchor;
    public Transform playerRed;
    public Transform playerArctic;
    private void Start()
    {
        //mapAnchor.position = new Vector3(-mapSize.x / 2, -mapSize.y / 2, 0);
    }
    private void Update()
    {
        if (playerRed.gameObject.activeSelf)
            playerImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 -playerRed.eulerAngles.y));
        else
            playerImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 -playerArctic.eulerAngles.y));

        playerImage.rectTransform.localPosition = new Vector3(offset.x + playerRed.position.x * playerRed.position.x / worldSize.x, offset.y + playerRed.position.z * playerRed.position.z / worldSize.y);
        //if (mapImage.rectTransform.localPosition.x < Screen.width / 2)
        //{
        //    playerImage.rectTransform.localPosition = new Vector3(mapImage.rectTransform.localPosition.x - Screen.width / 2, 0);
        //    mapImage.rectTransform.localPosition = new Vector3(Screen.width / 2, mapImage.rectTransform.localPosition.y);
        //}
        //else if (mapImage.rectTransform.localPosition.x > mapSize.x - Screen.width / 2)
        //{
        //    playerImage.rectTransform.localPosition = new Vector3(mapImage.rectTransform.localPosition.x - (mapSize.x - Screen.width / 2), 0);
        //    mapImage.rectTransform.localPosition = new Vector3(mapSize.x - Screen.width / 2, mapImage.rectTransform.localPosition.y);
        //}
        //else
        //    playerImage.rectTransform.localPosition = new Vector3(0, 0);
        //if (mapImage.rectTransform.localPosition.y < Screen.height / 2)
        //{
        //    playerImage.rectTransform.localPosition = new Vector3(playerImage.rectTransform.localPosition.x, mapImage.rectTransform.localPosition.y - Screen.height / 2, 0);
        //    mapImage.rectTransform.localPosition = new Vector3(mapImage.rectTransform.localPosition.x, Screen.height / 2);
        //}
        //else if (mapImage.rectTransform.localPosition.y > mapSize.y - Screen.height / 2)
        //{
        //    playerImage.rectTransform.localPosition = new Vector3(playerImage.rectTransform.localPosition.x, mapImage.rectTransform.localPosition.y - (mapSize.y - Screen.height / 2), 0);
        //    mapImage.rectTransform.localPosition = new Vector3(mapImage.rectTransform.localPosition.x, mapSize.y - Screen.height / 2);
        //}
        //else
        //    playerImage.rectTransform.localPosition = new Vector3(playerImage.rectTransform.localPosition.x, 0);
    }
}
