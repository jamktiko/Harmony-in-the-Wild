using UnityEngine;
using System.Collections.Generic;

public class MultiTerrainParticleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject container;
    public GameObject particleSystemPrefab;
    public Terrain[] terrains;
    public int targetTreeIndex = 0;
    public float yOffset = 5f;
    [Range(0f, 100f)] public float spawnChancePercentage = 100f;

    void Start()
    {
        if (terrains.Length == 0 || particleSystemPrefab == null || container == null)
        {
            Debug.LogError("Please assign all references in the Inspector.");
            return;
        }

        if (spawnChancePercentage < 0f || spawnChancePercentage > 100f)
        {
            Debug.LogError("Invalid spawnChancePercentage: " + spawnChancePercentage + ". Value must be between 0 and 100.");
            return;
        }

        SpawnParticlesOnTrees();
    }

    void SpawnParticlesOnTrees()
    {
        System.Random random = new System.Random();
        int totalSpawned = 0;

        foreach (Terrain terrain in terrains)
        {
            if (terrain == null)
                continue;

            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
            List<TreeInstance> matchingTrees = new List<TreeInstance>();

            foreach (TreeInstance tree in treeInstances)
            {
                if (tree.prototypeIndex == targetTreeIndex)
                {
                    matchingTrees.Add(tree);
                }
            }

            int totalMatchingTrees = matchingTrees.Count;
            int treesToSpawn = Mathf.RoundToInt((spawnChancePercentage / 100f) * totalMatchingTrees);

            Debug.Log("Terrain \"" + terrain.name + "\": " + spawnChancePercentage + "% selected. "
                      + totalMatchingTrees + " matching trees found; spawning on " + treesToSpawn + " trees.");

            ShuffleList(matchingTrees, random);

            for (int i = 0; i < treesToSpawn; i++)
            {
                TreeInstance selectedTree = matchingTrees[i];
                Vector3 treeWorldPosition = Vector3.Scale(selectedTree.position, terrain.terrainData.size) + terrain.transform.position;
                treeWorldPosition.y += yOffset;

                GameObject particleSystemInstance = Instantiate(particleSystemPrefab, treeWorldPosition, Quaternion.identity);
                particleSystemInstance.transform.SetParent(container.transform);

                totalSpawned++;
            }
        }

        Debug.Log("Successfully spawned " + totalSpawned + " particle system instance(s).");
    }

    void ShuffleList(List<TreeInstance> list, System.Random random)
    {
        int count = list.Count;
        for (int i = count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            TreeInstance temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
