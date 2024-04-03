using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    [SerializeField] internal float activeTime = 2f;

    [Header("Mesh Trail Settings")]
    [SerializeField] internal float meshRefreshRate = 0.1f;
    [SerializeField] internal float meshDestroyDelay = 0.5f;
    [SerializeField] internal Transform meshSpawnPosition;

    [Header("Shader Settings")]
    [SerializeField] internal Material spawnedMeshMaterial;
    [SerializeField] internal string shaderVarRef;
    [SerializeField] internal float shaderVarRate = 0.1f;
    [SerializeField] internal float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public void ActivateTrail()
    {
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                Mesh bakedMesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(bakedMesh);

                for (int submeshIndex = 0; submeshIndex < skinnedMeshRenderer.sharedMesh.subMeshCount; submeshIndex++)
                {
                    Mesh submesh = new Mesh();
                    int[] indices = bakedMesh.GetTriangles(submeshIndex);
                    submesh.vertices = bakedMesh.vertices;
                    submesh.normals = bakedMesh.normals;
                    submesh.uv = bakedMesh.uv;
                    submesh.SetTriangles(indices, 0);

                    GameObject submeshGameObject = new GameObject("SubmeshTrail");
                    submeshGameObject.transform.SetPositionAndRotation(meshSpawnPosition.position, meshSpawnPosition.rotation);
                    MeshRenderer meshRenderer = submeshGameObject.AddComponent<MeshRenderer>();
                    MeshFilter meshFilter = submeshGameObject.AddComponent<MeshFilter>();

                    meshFilter.mesh = submesh;
                    meshRenderer.material = spawnedMeshMaterial; // Make sure this material supports transparency

                    StartCoroutine(MeshTrailFadeOut(meshRenderer.material, 0, shaderVarRate, shaderVarRefreshRate));

                    Destroy(submeshGameObject, meshDestroyDelay);
                }
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator MeshTrailFadeOut(Material _Material, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = _Material.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            _Material.SetFloat(shaderVarRef, valueToAnimate);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
