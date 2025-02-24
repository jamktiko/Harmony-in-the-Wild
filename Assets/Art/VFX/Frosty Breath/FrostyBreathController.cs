using UnityEngine;
using UnityEngine.VFX;

public class FrostBreathController : MonoBehaviour
{
    public VisualEffect visualEffect; // Reference to the VisualEffect component

    void Start()
    {
        // Check if the VisualEffect component is assigned
        if (visualEffect == null)
        {
            visualEffect = GetComponent<VisualEffect>();
        }
    }

    // Example function to set the isRunning? property
    public void SetIsRunning(bool isRunning)
    {
        if (visualEffect != null)
        {
            visualEffect.SetBool("isRunning?", isRunning); // Set the exposed property
        }
    }

    // Example function to get the value of isRunning?
    public bool GetIsRunning()
    {
        if (visualEffect != null)
        {
            return visualEffect.GetBool("isRunning?");
        }
        return false;
    }
}
