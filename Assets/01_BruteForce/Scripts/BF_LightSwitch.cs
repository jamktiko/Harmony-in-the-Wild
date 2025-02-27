using UnityEngine;

public class BF_LightSwitch : MonoBehaviour
{
    public GameObject lightScene;
    public Material skyboxScene;

    private void OnEnable()
    {
        if (lightScene != null)
        {
            lightScene.SetActive(true);
        }
        if (skyboxScene != null)
        {
            RenderSettings.skybox = skyboxScene;
        }
    }

    private void OnDisable()
    {
        if (lightScene != null)
        {
            lightScene.SetActive(false);
        }
    }

}
