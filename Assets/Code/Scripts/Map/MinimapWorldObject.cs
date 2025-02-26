using UnityEngine;

public class MinimapWorldObject : MonoBehaviour
{
    [SerializeField]
    private bool followObject = false;
    [SerializeField]
    private Sprite minimapIcon;
    public Sprite MinimapIcon => minimapIcon;

    private void Start()
    {
        try
        {
            MinimapController.Instance.RegisterMinimapWorldObject(this, followObject);

        }
        catch (System.Exception)
        {
            Debug.LogAssertion("no Minimap in the scene");
        }
    }

    private void OnDestroy()
    {
        try
        {
            MinimapController.Instance.RemoveMinimapWorldObject(this);
        }
        catch (System.Exception)
        {
            Debug.LogAssertion("no Minimap in the scene");
        }

    }
}