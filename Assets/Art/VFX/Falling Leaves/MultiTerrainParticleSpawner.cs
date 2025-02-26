using System.Collections.Generic;
using System.Linq; // Required for filtering
using UnityEngine;

public class MultiTerrainParticleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject container;
    public GameObject particleSystemPrefab;
    public Terrain[] terrains;
    public int targetTreeIndex = 0;
    public float yOffset = 5f;
    [Range(0f, 100f)] public float spawnChancePercentage = 100f;

    void Awake()
    {
        if (terrains == null || terrains.Length == 0)
        {
            terrains = FindObjectsOfType<Terrain>()
                .Where(t => t.name.StartsWith("MainTerrain_"))
                .ToArray();

            if (terrains.Length == 0)
            {
                Debug.LogError("No terrains found with the specified naming pattern.");
            }
        }
    }

    void Start()
    {
        if (particleSystemPrefab == null || container == null)
        {
            Debug.LogError("Please assign all references in the Inspector.");
            return;
        }
        SpawnParticlesOnTrees();
    }

    void SpawnParticlesOnTrees()
    {
        System.Random random = new System.Random();
        foreach (Terrain terrain in terrains)
        {
            if (terrain == null) continue;

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

            ShuffleList(matchingTrees, random);

            for (int i = 0; i < treesToSpawn; i++)
            {
                TreeInstance selectedTree = matchingTrees[i];
                Vector3 treeWorldPosition = Vector3.Scale(selectedTree.position, terrain.terrainData.size) + terrain.transform.position;
                treeWorldPosition.y += yOffset;

                GameObject particleSystemInstance = Instantiate(particleSystemPrefab, treeWorldPosition, Quaternion.identity);
                particleSystemInstance.transform.SetParent(container.transform);
            }
        }
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
