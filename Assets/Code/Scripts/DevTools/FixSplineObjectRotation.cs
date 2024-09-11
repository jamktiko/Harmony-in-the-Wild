using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FixSplineObjectRotation : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    [ContextMenu("Fix Rotation")]
    public void FixRotation()
    {
        Debug.Log("Starting to fix rotation...");

        Spline spline = splineContainer.Splines[1];
        int knotCount = spline.Count;

        Debug.Log("Spline count is " + knotCount);
        
        for(int i = 0; i < knotCount; i++)
        {
            var currentKnot = spline.ToArray()[i];

            // how to access bezier type????
            currentKnot.Rotation = Quaternion.Euler(0, 0, 0);

            spline.SetKnot(i, currentKnot);
        }
    }
}
