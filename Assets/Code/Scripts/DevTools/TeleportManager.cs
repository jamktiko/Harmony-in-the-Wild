using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TeleportManager : MonoBehaviour
{
    [FormerlySerializedAs("teleportLocations")] [SerializeField] private List<TeleportButtonData> _teleportLocations;
    [FormerlySerializedAs("teleportTriggerButton")] [SerializeField] private GameObject _teleportTriggerButton;

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            foreach (TeleportButtonData location in _teleportLocations)
            {
                GameObject newButton = Instantiate(_teleportTriggerButton, transform);
                newButton.GetComponent<TeleportButton>().InitializeLocation(location.Position.position, location.Name);
            }
        }
    }

    [System.Serializable]
    public class TeleportButtonData
    {
        [FormerlySerializedAs("name")] public string Name;
        [FormerlySerializedAs("position")] public Transform Position;
    }
}
