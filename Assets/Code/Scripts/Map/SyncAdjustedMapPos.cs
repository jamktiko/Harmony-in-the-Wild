using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SyncAdjustedMapPos : MonoBehaviour
{
    [FormerlySerializedAs("playerImage")] public Image PlayerImage;
    [FormerlySerializedAs("mapImage")] public Image MapImage;
    [FormerlySerializedAs("mapSize")] public Vector2 MapSize;
    [FormerlySerializedAs("worldSize")] public Vector2 WorldSize;
    [FormerlySerializedAs("offset")] public Vector2 Offset;
    [FormerlySerializedAs("mapAnchor")] public RectTransform MapAnchor;
    [FormerlySerializedAs("playerRed")] public Transform PlayerRed;
    [FormerlySerializedAs("playerArctic")] public Transform PlayerArctic;

    private Matrix4x4 _transformationMatrix;
    private Transform _activeTransform;

    private void Start()
    {
        //mapAnchor.position = new Vector3(-mapSize.x / 2, -mapSize.y / 2, 0);
        CalculateTransformationMatrix();
    }
    private void Update()
    {
        if (PlayerRed.gameObject.activeSelf)
        {
            PlayerImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 - PlayerRed.eulerAngles.y));
            _activeTransform = PlayerRed;
        }
        else
        {
            PlayerImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 - PlayerArctic.eulerAngles.y));
            _activeTransform = PlayerArctic;
        }

        // Convert player's world position to map position
        Vector2 worldPos = new Vector2(_activeTransform.position.x, _activeTransform.position.z) + Offset;
        Vector3 mapPos = _transformationMatrix.MultiplyPoint3x4(worldPos);

        PlayerImage.rectTransform.localPosition = mapPos;

        //playerImage.rectTransform.localPosition = WorldPositionToMapPosition(activeTransform.position);

        //playerImage.rectTransform.localPosition = new Vector3(offset.x + playerRed.position.x * playerRed.position.x / worldSize.x, offset.y + playerRed.position.z * playerRed.position.z / worldSize.y);

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

    private void CalculateTransformationMatrix()
    {
        var translation = -MapSize / 2; // Center the map
        var scaleRatio = MapSize / WorldSize; // Scale world coordinates to map size

        _transformationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, scaleRatio);
    }
}
