using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// handles vegetation color changes
// sets the initial color on start, as well as triggers the changes when cinematics are played

public class VegetationColorController : MonoBehaviour
{
    [FormerlySerializedAs("leafMaterial")] [SerializeField] private Material _leafMaterial;
    [FormerlySerializedAs("pineLeafMaterial")] [SerializeField] private Material _pineLeafMaterial;
    [FormerlySerializedAs("deadColor")] [SerializeField] private Color _deadColor;
    [FormerlySerializedAs("deadPineColor")] [SerializeField] private Color _deadPineColor;
    [FormerlySerializedAs("aliveColor")] [SerializeField] private Color _aliveColor;

    private float _updateState = 0f;
    private float _updateAmount = 0.25f;

    private void Start()
    {
        InitializeShaderValues();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics += TriggerVegetationColorChanges;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics -= TriggerVegetationColorChanges;
    }

    private void InitializeShaderValues()
    {
        // check the current state of ToL progress; set the color update value to be the same
        int treeOfLifeState = TreeOfLifeState.Instance.GetTreeOfLifeState();
        _updateState = SetNewUpdateState(treeOfLifeState);

        // use Color.Lerp to detect the value between the two colors that matches the desired update state
        _leafMaterial.SetColor("_LeafColor", Color.Lerp(_deadColor, _aliveColor, _updateState));
        _pineLeafMaterial.SetColor("_LeafColor", Color.Lerp(_deadPineColor, _aliveColor, _updateState));
    }

    private void TriggerVegetationColorChanges()
    {
        StartCoroutine(SmoothVegetationColorUpdate());
    }

    private IEnumerator SmoothVegetationColorUpdate()
    {
        int treeOfLifeState = TreeOfLifeState.Instance.GetTreeOfLifeState();
        _updateState = SetNewUpdateState(treeOfLifeState);

        float currentUpdateAmount = 0f;

        // gradually calculate the new color for the vegetation based on the update state
        while (currentUpdateAmount < _updateAmount)
        {
            //Debug.Log("Vegetation color update state is: " + updateState);
            _updateState += 0.01f;
            currentUpdateAmount += 0.01f;

            _leafMaterial.SetColor("_LeafColor", Color.Lerp(_deadColor, _aliveColor, _updateState));
            _pineLeafMaterial.SetColor("_LeafColor", Color.Lerp(_deadPineColor, _aliveColor, _updateState));

            yield return new WaitForSeconds(0.1f);
        }
    }

    private float SetNewUpdateState(int treeOfLifeState)
    {
        float newState = 0;

        switch (treeOfLifeState)
        {
            case 0:
                newState = 0;
                break;

            case 1:
                newState = 0.25f;
                break;

            case 2:
                newState = 0.5f;
                break;

            case 3:
                newState = 0.75f;
                break;

            case 4:
                newState = 1f;
                break;
        }

        return newState;
    }
}