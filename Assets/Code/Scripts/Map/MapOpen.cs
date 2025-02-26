using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapOpen : MonoBehaviour
{
    [FormerlySerializedAs("mapPanel")] [SerializeField] GameObject _mapPanel;
    [FormerlySerializedAs("mapCam")] [SerializeField] GameObject _mapCam;
    [FormerlySerializedAs("PlayerDisplayCanvas")] [SerializeField] GameObject _playerDisplayCanvas;
    [FormerlySerializedAs("globalVolume")] [SerializeField] internal Volume _globalVolume;

    [FormerlySerializedAs("mapQuestMarkers")]
    [Header("Indicators")]
    [SerializeField] private Image[] _mapQuestMarkers;

    private UnityEngine.Rendering.Universal.DepthOfField _depthOfField;

    void Start()
    {
        CheckDepthOfField();
        StartCoroutine(ReEnableMarkers());
    }

    private void CheckDepthOfField()
    {
        if (_globalVolume.profile.TryGet(out _depthOfField))

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
        if (PlayerInputHandler.Instance.OpenMapInput.WasPressedThisFrame())
        {
            DialogueManager.Instance.CanStartDialogue = !DialogueManager.Instance.CanStartDialogue;

            ToggleMapVisibility();
            UpdateCursorState();
            ToggleDepthOfField();
        }
    }

    private void ToggleMapVisibility()
    {
        //mapCam.SetActive(!mapCam.activeInHierarchy);
        _mapPanel.SetActive(!_mapPanel.activeInHierarchy);
        //PlayerDisplayCanvas.SetActive(!PlayerDisplayCanvas.activeInHierarchy);
        if (_mapPanel.activeSelf)
            QuestManager.Instance.SetQuestMarkers(_mapQuestMarkers);
    }

    private void UpdateCursorState()
    {
        bool mapIsActive = _mapCam.activeInHierarchy;
        Cursor.lockState = mapIsActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mapIsActive;
    }

    private void ToggleDepthOfField()
    {
        if (_depthOfField != null)
        {
            _depthOfField.active = !_mapCam.activeInHierarchy;
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
        foreach (Transform child in _mapPanel.transform)
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
