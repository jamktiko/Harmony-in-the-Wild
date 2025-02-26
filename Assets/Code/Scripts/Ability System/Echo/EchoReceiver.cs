using UnityEngine;

public class EchoReceiver : MonoBehaviour
{
    [SerializeField] private GameObject _echoReceiverVFX;
    [SerializeField] private float _timeToHideEchoReceiverVFX = 1.1f;

    private void Start()
    {
        _echoReceiverVFX = transform.Find("EchoVFX").gameObject;

        if (_echoReceiverVFX == null)
        {
            Debug.Log($"No echo receiver VFX found for {gameObject.name}.");
        }
    }

    public void ObjectLocated()
    {
        _echoReceiverVFX.SetActive(true);
        Invoke(nameof(HideEffect), _timeToHideEchoReceiverVFX);
    }

    private void HideEffect()
    {
        _echoReceiverVFX.SetActive(false);
    }
}
