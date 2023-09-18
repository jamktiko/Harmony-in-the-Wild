using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int depth = 20;

    public int width = 2200;
    public int length = 2000;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.size = new Vector3(width, length, depth);

        terrainData.SetHeights(0, 0, GenerateHeights());
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = //insert image data
            }
        }
    }
}
