using UnityEngine;

public class TelegrabObject : MonoBehaviour
{
    public Material TelegrabMaterial;

    void Start()
    {
        TelegrabMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }
}
