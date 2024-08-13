using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIForTrailerRecording : MonoBehaviour
{
    private bool isVisible = true;

    void Update()
    {
        if (PlayerInputHandler.instance.DebugHideUI.WasPressedThisFrame())
        {
            ToggleVisibility();
        }
    }

    private void ToggleVisibility()
    {
        if (isVisible)
        {
            // hide UI
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        else
        {
            // show UI
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}