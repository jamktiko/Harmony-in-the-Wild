using UnityEngine;
public class SaturationTest : MonoBehaviour
{
    public string globalSaturation = "_Saturation";
    public float saturation {get; set;} = 1.0f;

    private void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            foreach (Material material in materials)
            {
                if (material.HasProperty(globalSaturation))
                {
                    material.SetFloat(globalSaturation, saturation);
                }
            }
        }
        Shader.SetGlobalFloat("_saturation", saturation);
    }

    public void UpdateSaturation()
    {
        Shader.SetGlobalFloat("_saturation", saturation);
    }
}
