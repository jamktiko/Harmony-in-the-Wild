using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class FixSplineObjectRotation : MonoBehaviour
{
    [FormerlySerializedAs("splineContainer")] [SerializeField] private SplineContainer _splineContainer;
    [FormerlySerializedAs("splineIndex")] [SerializeField] private int _splineIndex = 0;

    [ContextMenu("Fix Rotation")]
    public void FixRotation()
    {
        Debug.Log("Starting to fix rotation...");

        Spline spline = _splineContainer.Splines[_splineIndex];
        int knotCount = spline.Count;

        Debug.Log("Spline count is " + knotCount);

        for (int i = 0; i < knotCount; i++)
        {
            var currentKnot = spline.ToArray()[i];

            //spline.SetTangentMode(i, mode: TangentMode.Linear, BezierTangent.Out);
            currentKnot.Rotation = Quaternion.Euler(0, 0, 0);

            spline.SetKnot(i, currentKnot);
        }

        Debug.Log("Rotation fixing process ended.");
    }
}
