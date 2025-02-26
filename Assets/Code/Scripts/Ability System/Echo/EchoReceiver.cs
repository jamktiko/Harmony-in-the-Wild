using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoReceiver : MonoBehaviour
{
    [SerializeField] private GameObject echoReceiverVFX;
    [SerializeField] private float timeToHideEchoReceiverVFX = 1.1f;

    private void Start()
    {
        echoReceiverVFX = transform.Find("EchoVFX").gameObject;

        if(echoReceiverVFX == null)
        {
            Debug.Log($"No echo receiver VFX found for { gameObject.name }.");
        }
    }

    public void ObjectLocated()
    {
        echoReceiverVFX.SetActive(true);
        Invoke(nameof(HideEffect), timeToHideEchoReceiverVFX);
    }

    private void HideEffect()
    {
        echoReceiverVFX.SetActive(false);
    }
}
