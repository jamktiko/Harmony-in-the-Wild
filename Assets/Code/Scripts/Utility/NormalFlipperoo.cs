using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFlipperoo : MonoBehaviour
{
    public GameObject[] flippers;
    public Vector3[] dir;
    // Flips all normals of the assigned gameobjects' meshes in the requested direction. Good for creating mesh-based particle systems.
    // Operation seems to be PERMANENT, so ONLY use this on duplicate meshes, otherwise you'll screw up the prefab itself!
    void Start()
    {
        Mesh mesh;
        Vector3[] newNormals;
        for (int i = 0; i < flippers.Length; i++) 
        { 
            mesh = flippers[i].GetComponent<MeshFilter>().sharedMesh;
            newNormals = new Vector3[mesh.normals.Length];
            for (int j = 0; j < mesh.normals.Length; j++)
                newNormals[j] = dir[i];
            mesh.normals = newNormals;
        }
    }
}
