using UnityEngine;
using UnityEngine.VFX;

public class WaterExitEventController : MonoBehaviour
{
    [SerializeField] public VisualEffect vfxGraph;
    internal string beginExitWaterEventName = "OnExitWater";
    internal string shakeWaterLeftEventName = "OnShakeWaterLeft";
    internal string shakeWaterRightEventName = "OnShakeWaterRight";
    internal string endExitWaterEventName = "OnWaterExited";

    public void Awake()
    {
        vfxGraph.SendEvent(endExitWaterEventName);
    }

    public void BeginExitWater()
    {
        vfxGraph.SendEvent(beginExitWaterEventName);
    }

    public void ShakeWaterLeft()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(shakeWaterLeftEventName);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }

    public void ShakeWaterRight()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(shakeWaterRightEventName);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }

    public void EndExitWater()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(endExitWaterEventName);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }
}
