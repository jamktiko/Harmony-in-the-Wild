using System.Collections;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    public Material leafMaterial;
    public Material flowerMaterial;
    private float leafGrowIncrement = 0.5f;
    private float flowerGrowIncrement = 0.25f;
    private float duration = 10f; // Duration over which the growth occurs
    private bool isGrowing = false;

    void Start()
    {
        if (leafMaterial == null || flowerMaterial == null)
        {
            Debug.LogError("Materials not assigned!");
        }
    }

    public void TriggerGrowth()
    {
        if (isGrowing) return;

        float currentLeafGrow = leafMaterial.GetFloat("_Grow");
        if (currentLeafGrow < 1f)
        {
            StartCoroutine(SmoothGrow(leafMaterial, currentLeafGrow, Mathf.Min(currentLeafGrow + leafGrowIncrement, 1f)));
        }
        else
        {
            float currentFlowerGrow = flowerMaterial.GetFloat("_Grow");
            StartCoroutine(SmoothGrow(flowerMaterial, currentFlowerGrow, currentFlowerGrow + flowerGrowIncrement));
        }
    }

    private IEnumerator SmoothGrow(Material material, float startValue, float endValue)
    {
        isGrowing = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            material.SetFloat("_Grow", newValue);
            yield return null;
        }

        material.SetFloat("_Grow", endValue);
        isGrowing = false;
    }
}