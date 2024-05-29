using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerZoneEnabler : MonoBehaviour
{
    [Tooltip("State of ToL progress which enables this flower zone")]
    [SerializeField] private int enablingState;

    private void Start()
    {
        Invoke(nameof(SetVisibility), 0.01f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += SetVisibility;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= SetVisibility;
    }

    private void SetVisibility()
    {
        int currentState = TreeOfLifeState.instance.GetTreeOfLifeState();

        if(currentState >= enablingState)
        {
            Debug.Log("Show zone: " + gameObject.name);

            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
