using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainTreesToGameObjects : MonoBehaviour
{
    [ContextMenu("Extract")]
    public void Extract()
    {
        Debug.Log("TerrainTreesToGameObjects::Extract");
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

                // Instantiate a copy of the tree prefab at the calculated position
                GameObject treeObject = Instantiate(tree.prefab, worldPosition, Quaternion.identity);
                treeObject.name = tree.prefab.name + "_Instance_" + j;

                // Set tree's scale based on instance width and height scales
                treeObject.transform.localScale = new Vector3(instance.widthScale, instance.heightScale, instance.widthScale);

                // Set parent to keep hierarchy tidy and for easy management
                treeObject.transform.parent = terrain.transform;

                // Optional: Set layer to match either tree prefab layer or terrain layer
                if (terrain.preserveTreePrototypeLayers)
                    treeObject.layer = tree.prefab.layer;
                else
                    treeObject.layer = terrain.gameObject.layer;
            }
        }

        // Remove all tree instances from the terrain
        terrain.terrainData.treeInstances = new TreeInstance[0];

        Debug.Log("All tree instances have been removed from the terrain and copied as GameObjects.");
    }
}