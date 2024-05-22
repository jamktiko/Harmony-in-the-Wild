using UnityEngine;

public class GrowthTester : MonoBehaviour
{
    public GameObject objectToEnable;
    public GrowthController growthController;

    void Start()
    {
        if (objectToEnable == null || growthController == null)
        {
            Debug.LogError("Object to enable or GrowthController is not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            if (growthController != null)
            {
                growthController.TriggerGrowth();
            }
        }
    }
}