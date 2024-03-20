using UnityEngine;

public class MapOpen : MonoBehaviour
{
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject mapCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapCam.SetActive(!mapCam.activeInHierarchy);
            mapPanel.SetActive(!mapPanel.activeInHierarchy);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (!mapCam.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //unlock and lock all tp points for testing purposes wew
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Transform child in mapPanel.transform)
            {
                if (child.name != "Map")
                {
                    child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
                }
            }
        }
    }
}
