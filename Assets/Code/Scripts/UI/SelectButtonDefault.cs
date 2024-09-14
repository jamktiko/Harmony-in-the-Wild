using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonDefault : MonoBehaviour
{
    [SerializeField] Button primaryButton;
    // Start is called before the first frame update
    void Start()
    {
        primaryButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
