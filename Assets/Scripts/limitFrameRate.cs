using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}