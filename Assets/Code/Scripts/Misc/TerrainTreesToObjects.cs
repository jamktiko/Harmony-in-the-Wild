using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[RequireComponent(typeof(Terrain))]
public class TerrainTreesToPrefabInstances : MonoBehaviour
{
    [ContextMenu("Extract")]
    public void Extract()
    {
        Debug.Log("TerrainTreesToPrefabInstances::Extract");
        Terrain terrain = GetComponent<Terrain>();

        // Delete previously created tree GameObjects under the Terrain
        Transform[] transforms = terrain.GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            DestroyImmediate(transforms[i].gameObject);
        }

        Debug.Log("Tree prototypes count: " + terrain.terrainData.treePrototypes.Length);

        // Loop through each tree prototype on the terrain
        for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];
            GameObject prefab = tree.prefab;

            // Check if the tree prototype's prefab exists as an asset
            if (prefab == null || PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab)
            {
                Debug.LogWarning($"Tree prototype {i} does not have a valid prefab asset. Skipping.");
                continue;
            }

            // Get all instances matching the current prototype index
            TreeInstance[] instances = terrain.terrainData.treeInstances
                .Where(x => x.prototypeIndex == i).ToArray();

            Debug.Log("Tree prototype[" + i + "] instance count: " + instances.Length);

            // Loop through each tree instance of this prototype
            for (int j = 0; j < instances.Length; j++)
            {
                TreeInstance instance = instances[j];

                // Un-normalize position to world-space
                Vector3 worldPosition = Vector3.Scale(instance.position, terrain.terrainData.size) + terrain.GetPosition();

                // Instantiate a linked prefab instance at the calculated position
                GameObject treeObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab, terrain.transform);
                treeObject.name = prefab.name + "_Instance_" + j;

                // Set the scale based on the TreeInstance's width and height
                treeObject.transform.localScale = new Vector3(instance.widthScale, instance.heightScale, instance.widthScale);

                // Set position and parent to the terrain for organization
                treeObject.transform.position = worldPosition;
                treeObject.transform.parent = terrain.transform;

                // Optional: Set layer according to terrain or prototype layer
                if (terrain.preserveTreePrototypeLayers)
                    treeObject.layer = prefab.layer;
                else
                    treeObject.layer = terrain.gameObject.layer;
            }       
        }

        // Get only tree instances that should be kept (those that use a prefab that has )
        TreeInstance[] instancesToKeep = terrain.terrainData.treeInstances
            .Where(instance =>
            {
        // Get the prefab for the current tree instance's prototype index
        int prototypeIndex = instance.prototypeIndex;
                GameObject prefab = terrain.terrainData.treePrototypes[prototypeIndex].prefab;

        // Check if the prefab name contains "Grass"
        return prefab != null && prefab.name.Contains("Grass");
            })
            .ToArray();

        // Update the terrain's treeInstances with only the instances to keep
        terrain.terrainData.treeInstances = instancesToKeep;

        Debug.Log("Non-Grass tree instances have been removed, only 'Grass' trees are kept on the terrain.");
    }
}

#endif