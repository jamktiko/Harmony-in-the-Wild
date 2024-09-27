using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles vegetation color changes
// sets the initial color on start, as well as triggers the changes when cinematics are played

public class VegetationColorController : MonoBehaviour
{
    [SerializeField] private Material leafMaterial;
    [SerializeField] private Material pineLeafMaterial;
    [SerializeField] private Color deadColor;
    [SerializeField] private Color deadPineColor;
    [SerializeField] private Color aliveColor;

    private float updateState = 0f;
    private float updateAmount = 0.25f;

    private void Start()
    {
        InitializeShaderValues();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += TriggerVegetationColorChanges;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= TriggerVegetationColorChanges;
    }

    private void InitializeShaderValues()
    {
        // check the current state of ToL progress; set the color update value to be the same
        updateState = TreeOfLifeState.instance.GetTreeOfLifeState();

        // use Color.Lerp to detect the value between the two colors that matches the desired update state
        leafMaterial.SetColor("_LeafColor", Color.Lerp(deadColor, aliveColor, updateState));
        pineLeafMaterial.SetColor("_LeafColor", Color.Lerp(deadPineColor, aliveColor, updateState));
    }
    
    private void TriggerVegetationColorChanges()
    {
        StartCoroutine(SmoothVegetationColorUpdate());
    }

    private IEnumerator SmoothVegetationColorUpdate()
    {
        updateState = TreeOfLifeState.instance.GetTreeOfLifeState();

        float currentUpdateAmount = 0f;

        // gradually calculate the new color for the vegetation based on the update state
        while (currentUpdateAmount < updateAmount)
        {
            //Debug.Log("Vegetation color update state is: " + updateState);
            updateState += 0.01f;
            currentUpdateAmount += 0.01f;

            leafMaterial.SetColor("_LeafColor", Color.Lerp(deadColor, aliveColor, updateState));
            pineLeafMaterial.SetColor("_LeafColor", Color.Lerp(deadPineColor, aliveColor, updateState));

            yield return new WaitForSeconds(0.1f);
        }
    }
}