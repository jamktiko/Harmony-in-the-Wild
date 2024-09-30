using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MapOpen : MonoBehaviour
{
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject TeleportPanel;
    [SerializeField] GameObject mapCam;
    [SerializeField] GameObject PlayerDisplayCanvas;
    [SerializeField] internal Volume globalVolume;

    private UnityEngine.Rendering.Universal.DepthOfField depthOfField;

    void Start()
    {
        CheckDepthOfField();
        StartCoroutine(ReEnableMarkers());
    }

    private void CheckDepthOfField()
    {
        if (globalVolume.profile.TryGet(out depthOfField))

        {
            //Debug.Log("Depth of Field found in the Global Volume.");
        }
        else
        {
            Debug.LogError("Depth of Field not found in the Global Volume.");
        }
    }

    void Update()
    {
        HandleMapToggle();
        HandleDebugFeatures();
    }

    private void HandleMapToggle()
    {
        if (PlayerInputHandler.instance.OpenMapInput.WasPressedThisFrame())
        {
            DialogueManager.instance.canStartDialogue = !DialogueManager.instance.canStartDialogue;

            ToggleMapVisibility();
            UpdateCursorState();
            ToggleDepthOfField();
        }
    }

    private void ToggleMapVisibility()
    {
        mapCam.SetActive(!mapCam.activeInHierarchy);
        mapPanel.SetActive(!mapPanel.activeInHierarchy);
        TeleportPanel.SetActive(!TeleportPanel.activeInHierarchy);
        //PlayerDisplayCanvas.SetActive(!PlayerDisplayCanvas.activeInHierarchy);
    }

    private void UpdateCursorState()
    {
        bool mapIsActive = mapCam.activeInHierarchy;
        Cursor.lockState = mapIsActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mapIsActive;
    }

    private void ToggleDepthOfField()
    {
        if (depthOfField != null)
        {
            depthOfField.active = !mapCam.activeInHierarchy;
        }
    }

    private void HandleDebugFeatures()
    {
        //if (PlayerInputHandler.instance.JumpInput.WasPressedThisFrame())
        //{
        //    ToggleDebugFeatures();
        //}
    }

    private void ToggleDebugFeatures()
    {
        foreach (Transform child in mapPanel.transform)
        {
            if (child.name != "Map")
            {
                child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
            }
        }
    }

    private IEnumerator ReEnableMarkers()
    {
        GetComponent<QuestMarkers>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<QuestMarkers>().enabled = true;
    }
}
