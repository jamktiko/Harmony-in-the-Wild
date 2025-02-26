using System.Collections.Generic;
using UnityEngine;

public class ToggleUIForTrailerRecording : MonoBehaviour
{
    private bool _isVisible = true;
    private bool _previousObjectsListed = false;

    private List<GameObject> _previousObjects = new List<GameObject>();

    void Update()
    {
        //if (PlayerInputHandler.instance.DebugHideUI.WasPressedThisFrame())
        //{
        //    ToggleVisibility();
        //}
    }

    private void ToggleVisibility()
    {
        if (_isVisible)
        {
            // hide UI
            foreach (Transform child in transform)
            {
                if (!_previousObjectsListed && child.gameObject.activeInHierarchy)
                {
                    _previousObjects.Add(child.gameObject);
                }

                child.gameObject.SetActive(false);
            }

            _previousObjectsListed = true;
            _isVisible = false;
        }

        else
        {
            // show UI
            foreach (GameObject child in _previousObjects)
            {
                child.gameObject.SetActive(true);
            }

            _isVisible = true;
        }
    }
}