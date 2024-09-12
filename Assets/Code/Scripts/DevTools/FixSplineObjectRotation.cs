using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FixSplineObjectRotation : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private int splineIndex = 0;

    [ContextMenu("Fix Rotation")]
    public void FixRotation()
    {
        Debug.Log("Starting to fix rotation...");

        Spline spline = splineContainer.Splines[splineIndex];
        int knotCount = spline.Count;

        Debug.Log("Spline count is " + knotCount);
        
        for(int i = 0; i < knotCount; i++)
        {
            var currentKnot = spline.ToArray()[i];

            spline.SetTangentMode(i, mode: TangentMode.Linear, BezierTangent.Out);
            currentKnot.Rotation = Quaternion.Euler(0, 0, 0);

            spline.SetKnot(i, currentKnot);
        }

        Debug.Log("Rotation fixing process ended.");
    }
}
