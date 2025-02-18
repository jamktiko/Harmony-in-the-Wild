using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoReceiver : MonoBehaviour
{
    [SerializeField] private GameObject objectFoundEffect;
    [SerializeField] private float timeToHideEffect = 1.1f;

    public void ObjectLocated()
    {
        objectFoundEffect.SetActive(true);
        Invoke(nameof(HideEffect), timeToHideEffect);
    }

    private void HideEffect()
    {
        objectFoundEffect.SetActive(false);
    }
}
