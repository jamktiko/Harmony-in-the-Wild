using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDevVisualsOnPlay : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
}
