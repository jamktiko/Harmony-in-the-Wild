using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegrabObject : MonoBehaviour
{
    public Material telegrabMaterial;
    // Start is called before the first frame update
    void Start()
    {
        telegrabMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
