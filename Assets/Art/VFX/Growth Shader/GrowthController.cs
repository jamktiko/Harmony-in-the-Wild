using System.Collections;
using UnityEngine;

/* This script controls the growth of the leaf and flower materials. 
   Modify the growth increments and duration as needed.
 */

public class GrowthController : MonoBehaviour
{
    public Material leafMaterial;
    public Material flowerMaterial;
    private float leafGrowIncrement = 0.5f;
    private float flowerGrowIncrement = 0.5f;
    private float duration = 10f;
    private bool isGrowing = false;

    [Header("VS Config")]
    [SerializeField] private bool isVerticalSliceScene = false;

    private void Start()
    {
        SetGrowthValues();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += TriggerGrowth;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= TriggerGrowth;
    }

    private void Update()
    {
        /*//NOTE ONLY FOR TRAILER RECORDING, DELETE LATER !!
        if (PlayerInputHandler.instance.DebugDeleteSaveInput.WasPerformedThisFrame())
        {
            flowerMaterial.SetFloat("_Grow", 0);
            StartCoroutine(SmoothGrow(leafMaterial, 0, 1));
            Invoke(nameof(StartFlowerGrow), duration);
        }*/
    }

    private void StartFlowerGrow()
    {
        StartCoroutine(SmoothGrow(flowerMaterial, 0, 1));
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

        // if in the vertical slice, show demo end after the first ToL cinematics
        if (isVerticalSliceScene)
        {
            GameEventsManager.instance.uiEvents.ShowLoadingScreen("DemoEnd");
        }
    }

    public void SaveGrowthValues()
    {
        float leafGrow = leafMaterial.GetFloat("_Grow");
        float flowerGrow = flowerMaterial.GetFloat("_Grow");
        PlayerPrefs.SetFloat("LeafGrow", leafGrow);
        PlayerPrefs.SetFloat("FlowerGrow", flowerGrow);
        PlayerPrefs.Save();
    }

    private void SetGrowthValues()
    {
        if (leafMaterial == null || flowerMaterial == null)
        {
            Debug.LogError("Materials not assigned!");
            return;
        }

        int currentState = TreeOfLifeState.instance.GetTreeOfLifeState();

        float leafGrow = 0;
        float flowerGrow = 0;

        switch (currentState)
        {
            case 0:
                leafGrow = 0;
                flowerGrow = 0;
                break;

            case 1:
                leafGrow = 0.5f;
                flowerGrow = 0;
                break;

            case 2:
                leafGrow = 1f;
                flowerGrow = 0;
                break;

            case 3:
                leafGrow = 1f;
                flowerGrow = 0.5f;
                break;

            case 4:
                leafGrow = 1f;
                flowerGrow = 1f;
                break;

            default:
                Debug.Log("Error in initializing the growth shader. State of ToL out of bounds.");
                break;
        }

        InitializeMaterials(leafGrow, flowerGrow);
    }

    private void InitializeMaterials(float leafGrow, float flowerGrow)
    {
        leafMaterial.SetFloat("_Grow", leafGrow);
        flowerMaterial.SetFloat("_Grow", flowerGrow);
    }
}
