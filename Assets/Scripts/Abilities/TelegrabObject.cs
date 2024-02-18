using UnityEngine;

public class TelegrabObject : MonoBehaviour
{
    public Material telegrabMaterial;

    void Start()
    {
        telegrabMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }
}
