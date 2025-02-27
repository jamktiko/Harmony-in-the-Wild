using System.Collections.Generic;
using UnityEngine;

public class SnowWaypoints : MonoBehaviour
{
    //Gizmo variables
    private Vector3 _gizmoCubeSize = new Vector3(1, 1, 1);

    //Mesh variables
    private Mesh _mesh;
    public Material Material;

    //verts
    public List<Vector3> VertList = new List<Vector3>();
    private Vector3[] _verts;
    private int _currentIndex = 0;

    //uv
    private Vector2[] _uv;

    //tris
    private int[] _tris;
    private int _triList;

    private void OnDrawGizmos() //Gizmos (yes, it's obvious, but this is a visual aid)
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(t.position, _gizmoCubeSize);
        }
    }
    private void Awake() //Mesh thingys
    {
        CollectChildPositions();
        //Add position data to list

        //vertices
        _verts = new Vector3[VertList.Count];
        foreach (Vector3 value in VertList)
        {
            _verts[_currentIndex] = value;
            _currentIndex++;
        }

        //triangles
        if (VertList.Count >= 3)
        {
            _triList = 3 + (VertList.Count - 3) * 3;
            _tris = new int[_triList];

            //finally actually create the tri positions
            for (int i = 0, j = 0; i < _triList;)
            {
                _tris[i] = j;
                _tris[i + 1] = j + 1;
                _tris[i + 2] = j + 2;

                Debug.Log("First pass" + _tris[i] + _tris[i + 1] + _tris[i + 2]);

                i = i + 3;
                j++;

                if (i < _triList)
                {
                    _tris[i] = j;
                    _tris[i + 1] = j + 2;
                    _tris[i + 2] = j + 1;

                    Debug.Log("Second pass" + _tris[i] + _tris[i + 1] + _tris[i + 2]);
                    i = i + 3;
                    j++;
                }
            }
        }
        else
        {
            return;
        }
    }

    private void Start() //Mesh thingys
    {
        _mesh.Clear();
        _mesh = new Mesh
        {
            vertices = _verts,
            triangles = _tris,
            uv = _uv
        };

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GameObject meshObject = new GameObject("GeneratedMesh");
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();

        meshFilter.mesh = _mesh;

        meshObject.AddComponent<MeshRenderer>();
    }

    private void CollectChildPositions()
    {
        VertList.Clear();
        foreach (Transform t in transform)
        {
            VertList.Add(t.position);
        }
    }
}
