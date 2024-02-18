using UnityEngine;

public class MapOpen : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] GameObject mapCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapCam.SetActive(!mapCam.activeInHierarchy);
            map.SetActive(!map.activeInHierarchy);
        }
    }
}
