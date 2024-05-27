using System.Collections;
using UnityEngine;

/* This script controls the growth of the leaf and flower materials. 
   Modify the growth increments and duration as needed.
   Change playerprefs to a save system if needed.
 */

public class GrowthController : MonoBehaviour
{
    public Material leafMaterial;
    public Material flowerMaterial;
    private float leafGrowIncrement = 0.5f;
    private float flowerGrowIncrement = 0.25f;
    private float duration = 10f;
    private bool isGrowing = false;

    void Start()
    {
        if (leafMaterial == null || flowerMaterial == null)
        {
            Debug.LogError("Materials not assigned!");
        }
        float leafGrow = PlayerPrefs.GetFloat("LeafGrow", 0f);
        float flowerGrow = PlayerPrefs.GetFloat("FlowerGrow", 0f);
        leafMaterial.SetFloat("_Grow", leafGrow);
        flowerMaterial.SetFloat("_Grow", flowerGrow);
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

    public void SaveGrowthValues()
    {
        float leafGrow = leafMaterial.GetFloat("_Grow");
        float flowerGrow = flowerMaterial.GetFloat("_Grow");
        PlayerPrefs.SetFloat("LeafGrow", leafGrow);
        PlayerPrefs.SetFloat("FlowerGrow", flowerGrow);
        PlayerPrefs.Save();
    }
}
