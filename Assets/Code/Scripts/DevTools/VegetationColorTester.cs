using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationColorTester : MonoBehaviour
{
    [SerializeField] private Material leafMaterial;
    [SerializeField] private Color deadColor;
    [SerializeField] private Color aliveColor;

    private float updateState = 0f;
    private float updateAmount = 0.25f;

    private void Start()
    {
        InitializeShaderValues();
    }

    private void Update()
    {
        if (PlayerInputHandler.instance.DebugVegetationColorChanger.WasPressedThisFrame())
        {
            if(updateState < 1f)
            {
                StartCoroutine(SmoothVegetationColorUpdate());
            }

            else
            {
                updateState = 0f;
                StartCoroutine(SmoothVegetationColorUpdate());
            }
        }
    }

    private void UpdateVegetationColor()
    {
        updateState += 0.25f;
        Color currentColor = Color.Lerp(deadColor, aliveColor, updateState);

        leafMaterial.SetColor("_LeafColor", currentColor);
    }

    private void InitializeShaderValues()
    {
        updateState = 0f;
        //updateState = TreeOfLifeState.instance.GetTreeOfLifeState();

        Color currentColor = Color.Lerp(deadColor, aliveColor, updateState);

        leafMaterial.SetColor("_LeafColor", currentColor);
    }
    
    private IEnumerator SmoothVegetationColorUpdate()
    {
        float currentUpdateAmount = 0f;

        while (currentUpdateAmount < updateAmount)
        {
            updateState += 0.01f;
            currentUpdateAmount += 0.01f;

            Color currentColor = Color.Lerp(deadColor, aliveColor, updateState);

            leafMaterial.SetColor("_LeafColor", currentColor);

            yield return new WaitForSeconds(0.01f);
        }
    }
}