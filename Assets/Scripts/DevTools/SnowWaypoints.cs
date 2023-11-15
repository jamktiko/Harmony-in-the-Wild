using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWaypoints : MonoBehaviour
{
    //Gizmo variables
    private Vector3 cubeSize = new Vector3 (1, 1, 1);

    //Mesh variables
    private Mesh mesh;
    public Material material;

    public List<Vector3> vertList = new List<Vector3>();
    private int i = 0;

    private Vector3[] verts;
    private Vector2[] uv;
    private int[] tris;

    private int triList;
    private void OnDrawGizmos() //Gizmos (yes, it's obvious, but this is a visual aid)
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(t.position, cubeSize);
        }
    }
    private void Awake() //Mesh thingys
    {
        CollectChildPositions();
        //Add position data to list

        //vertices
        verts = new Vector3[vertList.Count];
        foreach(Vector3 value in vertList)
        {
            verts[i] = value;
            i++;
        }

        //triangles
        if (vertList.Count >= 3)
        {
            triList = 3 + (vertList.Count - 3) * 3;
            tris = new int[triList];
        }
        else
        {
            return;
        }

        //finally actually create the tri positions
        for (int i = 0, j = 0; i < vertList.Count;)
        {
            tris[i] = j;
            tris[i + 1] = j + 1;
            tris[i + 2] = j + 2;

            i = i + 3;
            j++;

            if (i < vertList.Count)
            {
                tris[i] = j;
                tris[i + 1] = j + 2;
                tris[i + 2] = j + 1;

                i = i + 3;
                j++;
            }
        }
    }

    private void Start() //Mesh thingys
    {
        mesh.Clear();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = tris;

        mesh.RecalculateNormals();
    }

    void CollectChildPositions()
    {
        vertList.Clear();
        foreach(Transform t in transform)
        {
            vertList.Add(t.position);
        }
    }
}
