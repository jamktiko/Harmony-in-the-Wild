using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] GameObject mapcam;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapcam.SetActive(!mapcam.activeInHierarchy);
            map.SetActive(!map.activeInHierarchy);
        }
    }
}
