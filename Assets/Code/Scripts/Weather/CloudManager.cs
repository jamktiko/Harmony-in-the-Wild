using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Mesh GenCloudMesh(int complexity = 8, float height = 2, float xScale = 10, float zScale = 16)
    {
        Mesh mesh = new Mesh();
        float2[] points = new float2[complexity];
        for (int i = 0; i < points.Length; i++)
        {

        }


        return mesh;
    }
}
