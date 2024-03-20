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

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (!mapCam.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
