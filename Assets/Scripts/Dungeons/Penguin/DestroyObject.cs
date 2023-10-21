using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float destructionTime;

    private void OnEnable()
    {
        Destroy(gameObject, destructionTime);
    }
}
