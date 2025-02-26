using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private List<TeleportButtonData> teleportLocations;
    [SerializeField] private GameObject teleportTriggerButton;

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            foreach (TeleportButtonData location in teleportLocations)
            {
                GameObject newButton = Instantiate(teleportTriggerButton, transform);
                newButton.GetComponent<TeleportButton>().InitializeLocation(location.position.position, location.name);
            }
        }
    }

    [System.Serializable]
    public class TeleportButtonData
    {
        public string name;
        public Transform position;
    }
}
