using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationColorTester : MonoBehaviour
{
    [SerializeField] private Material leafMaterial;
    [SerializeField] private Color deadColor;
    [SerializeField] private Color aliveColor;

    private float updateState = 0f;

    private void Start()
    {
        InitializeShaderValues();
    }

    private void Update()
    {
        if (PlayerInputHandler.instance.OpenMapInput.WasPressedThisFrame() && updateState < 1f)
        {
            UpdateVegetationColor();    
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
        updateState = (float)TreeOfLifeState.instance.GetTreeOfLifeState();

        Color currentColor = Color.Lerp(deadColor, aliveColor, updateState);

        leafMaterial.SetColor("_LeafColor", currentColor);
    }
}
