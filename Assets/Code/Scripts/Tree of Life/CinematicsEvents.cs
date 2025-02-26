using System;
using UnityEngine;

public class CinematicsEvents
{
    public event Action OnStartCinematics;

    public void StartCinematics()
    {
        OnStartCinematics?.Invoke();
    }

    public event Action OnEndCinematics;

    public void EndCinematics()
    {
        OnEndCinematics?.Invoke();
    }
}
