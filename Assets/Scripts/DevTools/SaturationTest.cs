using UnityEngine;
public class SaturationTest : MonoBehaviour
{
    public string globalSaturation = "_Saturation";
    public float Saturation {get; set;} = 1.0f;

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
                    material.SetFloat(globalSaturation, Saturation);
                }
            }
        }
        Shader.SetGlobalFloat("_saturation", Saturation);
    }

    public void UpdateSaturation()
    {
        Shader.SetGlobalFloat("_saturation", Saturation);
    }
}
