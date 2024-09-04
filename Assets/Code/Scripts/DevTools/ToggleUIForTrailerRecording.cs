using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIForTrailerRecording : MonoBehaviour
{
    private bool isVisible = true;
    private bool previousObjectsListed = false;

    private List<GameObject> previousObjects = new List<GameObject>();

    void Update()
    {
        //if (PlayerInputHandler.instance.DebugHideUI.WasPressedThisFrame())
        //{
        //    ToggleVisibility();
        //}
    }

    private void ToggleVisibility()
    {
        if (isVisible)
        {
            // hide UI
            foreach(Transform child in transform)
            {
                if (!previousObjectsListed && child.gameObject.activeInHierarchy)
                {
                    previousObjects.Add(child.gameObject);
                }

                child.gameObject.SetActive(false);
            }

            previousObjectsListed = true;
            isVisible = false;
        }

        else
        {
            // show UI
            foreach(GameObject child in previousObjects)
            {
                child.gameObject.SetActive(true);
            }

            isVisible = true;
        }
    }
}