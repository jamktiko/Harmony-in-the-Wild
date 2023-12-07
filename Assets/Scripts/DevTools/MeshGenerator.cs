using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    //Gizmos (visual aid)
    #region Gizmos
    private Vector3 cubeSize = new Vector3(1, 1, 1);
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(t.position, cubeSize);
        }
    }
    #endregion

    //Mesh generation (visual aid)
    #region Mesh generation

    protected MeshFilter meshFilter;
    protected Mesh mesh;

    //verts
    public List<Vector3> vertList = new List<Vector3>();
    private Vector3[] verts;
    private int i = 0;

    //tris
    private int[] tris;
    private int triListCount;

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "GeneratedMesh";

        mesh.vertices = GetVerts();
        mesh.triangles = GetTris();
        Debug.Log("Got vert and tri data");

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private Vector3[] GetVerts()
    {
        CollectChildPositions();

        verts = new Vector3[vertList.Count];
        foreach (Vector3 value in vertList)
        {
            verts[i] = value;
            i++;
        }
        return verts;
    }
    private int[] GetTris()
    {
        if (vertList.Count >= 3)
        {
            triListCount = 3 + (vertList.Count - 3) * 3;
            tris = new int[triListCount];

            //finally actually create the tri positions
            for (int i = 0, j = 0; i < triListCount;)
            {
                tris[i] = j;
                tris[i + 1] = j + 1;
                tris[i + 2] = j + 2;

                i = i + 3;
                j++;

                if (i < triListCount)
                {
                    tris[i] = j;
                    tris[i + 1] = j + 2;
                    tris[i + 2] = j + 1;

                    i = i + 3;
                    j++;
                }
            }
            return tris;
        }
        else
        {
            return null;
        }
    }

    void CollectChildPositions()
    {
        vertList.Clear();
        foreach (Transform t in transform)
        {
            vertList.Add(t.position);
        }
    }
    #endregion
}
