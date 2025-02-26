using UnityEngine;
using UnityEngine.Serialization;

public class MinimapWorldObject : MonoBehaviour
{
    [FormerlySerializedAs("followObject")] [SerializeField]
    private bool _followObject = false;
    [FormerlySerializedAs("minimapIcon")] [SerializeField]
    private Sprite _minimapIcon;
    public Sprite minimapIcon => _minimapIcon;

    private void Start()
    {
        try
        {
            MinimapController.Instance.RegisterMinimapWorldObject(this, _followObject);

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