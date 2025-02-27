using System.Collections;
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

    // Coroutine to activate and manage the visual trail effect for a given duration.
    private IEnumerator ActivateTrail(float timeActive)
    {
        // Loop for the duration of the trail activity.
        while (timeActive > 0)
        {
            // Decrement the active time by the refresh rate for each loop iteration.
            timeActive -= meshRefreshRate;

            // Retrieve all SkinnedMeshRenderer components from this GameObject and its children. These are used to create the trail meshes.
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                // Create a new mesh that represents the current pose of the skinned mesh renderer.
                Mesh bakedMesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(bakedMesh);

                // Iterate through each submesh to create individual trails.
                for (int submeshIndex = 0; submeshIndex < skinnedMeshRenderer.sharedMesh.subMeshCount; submeshIndex++)
                {
                    // Create a new mesh for the submesh to isolate it for the trail effect.
                    Mesh submesh = new Mesh();
                    int[] indices = bakedMesh.GetTriangles(submeshIndex);
                    submesh.vertices = bakedMesh.vertices;
                    submesh.normals = bakedMesh.normals;
                    submesh.uv = bakedMesh.uv;
                    submesh.SetTriangles(indices, 0);

                    // Instantiate a new GameObject to apply the trail mesh to.
                    GameObject submeshGameObject = new GameObject("SubmeshTrail");
                    submeshGameObject.transform.SetPositionAndRotation(meshSpawnPosition.position, meshSpawnPosition.rotation);
                    MeshRenderer meshRenderer = submeshGameObject.AddComponent<MeshRenderer>();
                    MeshFilter meshFilter = submeshGameObject.AddComponent<MeshFilter>();

                    // Assign the submesh to the MeshFilter and set the material for rendering.
                    meshFilter.mesh = submesh;
                    meshRenderer.material = spawnedMeshMaterial;

                    // Start a coroutine to fade out the mesh material over time.
                    StartCoroutine(MeshTrailFadeOut(meshRenderer.material, 0, shaderVarRate, shaderVarRefreshRate));

                    // Destroy the trail GameObject after a delay to clean up the scene.
                    Destroy(submeshGameObject, meshDestroyDelay);
                }
            }

            // Wait for a specified time before creating the next set of trail meshes.
            yield return new WaitForSeconds(meshRefreshRate);
        }

        // Mark the trail as inactive once the duration expires.
        isTrailActive = false;
    }

    // Coroutine to gradually fade out the material of a mesh by animating a shader property.
    private IEnumerator MeshTrailFadeOut(Material _Material, float goal, float rate, float refreshRate)
    {
        // Retrieve the current value of the shader property to be animated.
        float valueToAnimate = _Material.GetFloat(shaderVarRef);

        // Continuously animate the shader property until it reaches the goal.
        while (valueToAnimate > goal)
        {
            // Decrease the property value by the specified rate.
            valueToAnimate -= rate;
            _Material.SetFloat(shaderVarRef, valueToAnimate);

            // Wait for a specified refresh rate before the next animation step.
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
