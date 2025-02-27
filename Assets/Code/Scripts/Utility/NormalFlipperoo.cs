using UnityEngine;
using UnityEngine.Serialization;

public class NormalFlipperoo : MonoBehaviour
{
    [FormerlySerializedAs("flippers")] public GameObject[] Flippers;
    [FormerlySerializedAs("dir")] public Vector3[] Dir;
    // Flips all normals of the assigned gameobjects' meshes in the requested direction. Good for creating mesh-based particle systems.
    // Operation seems to be PERMANENT, so ONLY use this on duplicate meshes, otherwise you'll screw up the prefab itself!
    private void Start()
    {
        for (int i = 0; i < Flippers.Length; i++)
        {
            var mesh = Flippers[i].GetComponent<MeshFilter>().sharedMesh;
            var newNormals = new Vector3[mesh.normals.Length];
            for (int j = 0; j < mesh.normals.Length; j++)
                newNormals[j] = Dir[i];
            mesh.normals = newNormals;
        }
    }
}
