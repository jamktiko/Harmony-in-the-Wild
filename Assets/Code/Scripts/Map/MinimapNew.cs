using UnityEngine;
using UnityEngine.Serialization;

public class MinimapNew : MonoBehaviour
{
    [FormerlySerializedAs("marker")] public RectTransform Marker; //player pointer image
    [FormerlySerializedAs("mapImage")] public RectTransform MapImage;//Map screenshot used in canvas
    [FormerlySerializedAs("playerReference")] public Transform PlayerReference;//player
    [FormerlySerializedAs("mapEdges")] public Transform[] MapEdges;//4 edges, make sure its in rectangle.
    [FormerlySerializedAs("offset")] public Vector2 Offset;//Adjust the value to match you map

    private Vector2 _mapDimentions;
    private Vector2 _areaDimentions;

    private void Start()
    {
        //for (int i = 0; i < mapEdges.Length; i++)
        //    for (int j = i + 1; j < mapEdges.Length; j++)
        //    {
        //        if (mapEdges[j].position.x < mapEdges[i].position.x || mapEdges[j].position.z < mapEdges[i].position.z)
        //        {
        //            Transform temp = mapEdges[j];
        //            mapEdges[j] = mapEdges[i];
        //            mapEdges[i] = temp;
        //        }
        //    }
        _mapDimentions = new Vector2(MapImage.sizeDelta.x, MapImage.sizeDelta.y);
        _areaDimentions.x = MapEdges[1].position.x - MapEdges[0].position.x;
        _areaDimentions.y = MapEdges[2].position.z - MapEdges[0].position.z;
    }

    private void Update()
    {
        SetMarketPosition();
    }

    private void SetMarketPosition()
    {
        Vector3 distance = PlayerReference.position - MapEdges[0].position;
        Vector2 coordinates = new Vector2(distance.x / _areaDimentions.x, distance.z / _areaDimentions.y);
        Marker.anchoredPosition = new Vector2(coordinates.x * _mapDimentions.x, coordinates.y * _mapDimentions.y) + Offset;
        Marker.rotation = Quaternion.Euler(new Vector3(0, 0, -PlayerReference.eulerAngles.y + 180));
    }
}