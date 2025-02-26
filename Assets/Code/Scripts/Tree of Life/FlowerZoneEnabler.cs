using UnityEngine;
using UnityEngine.Serialization;

public class FlowerZoneEnabler : MonoBehaviour
{
    [FormerlySerializedAs("enablingState")]
    [Tooltip("State of ToL progress which enables this flower zone")]
    [SerializeField] private int _enablingState;

    private void Start()
    {
        Invoke(nameof(SetVisibility), 0.01f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics += SetVisibility;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics -= SetVisibility;
    }

    private void SetVisibility()
    {
        int currentState = TreeOfLifeState.Instance.GetTreeOfLifeState();

        if (currentState >= _enablingState)
        {
            //Debug.Log("Show zone: " + gameObject.name);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
