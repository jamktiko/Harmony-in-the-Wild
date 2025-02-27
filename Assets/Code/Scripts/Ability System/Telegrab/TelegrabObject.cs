using UnityEngine;

public class TelegrabObject : MonoBehaviour
{
    public Material TelegrabMaterial;

    private void Start()
    {
        TelegrabMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }
}
