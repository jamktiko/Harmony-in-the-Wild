using UnityEngine;

public class MultiTerrainParticleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject container;
    public GameObject particleSystemPrefab;
    public Terrain[] terrains;
    public int targetTreeIndex = 0;
    public float yOffset = 5f;

    void Start()
    {
        if (terrains.Length == 0 || particleSystemPrefab == null || container == null)
        {
            Debug.LogError("Please assign all references in the Inspector.");
            return;
        }
        SpawnParticlesOnTrees();
    }

    void SpawnParticlesOnTrees()
    {
        foreach (Terrain terrain in terrains)
        {
            if (terrain == null) continue;

            // Get all tree instances from the current terrain
            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

            foreach (TreeInstance tree in treeInstances)
            {
                // Check if the tree matches the target index
                if (tree.prototypeIndex == targetTreeIndex)
                {
                    Vector3 treeWorldPosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
                    treeWorldPosition.y += yOffset;
                    GameObject particleSystemInstance = Instantiate(particleSystemPrefab, treeWorldPosition, Quaternion.identity);
                    particleSystemInstance.transform.SetParent(container.transform);
                }
            }
        }
    }
}
