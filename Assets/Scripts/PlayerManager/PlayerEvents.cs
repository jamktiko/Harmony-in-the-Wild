using System;

public class PlayerEvents
{
    public event Action<int> OnExperienceGained;

    // refer to this event when GETTING new experience for example after completing a quest
    public void ExperienceGained(int newExperience)
    {
        OnExperienceGained?.Invoke(newExperience);
    }

    public event Action<int> OnExperienceChanged;

    // refer to this event when experience HAS BEEN UPDATED and the results need to be shown for example in UI
    public void ExperienceChanged(int currentExperience)
    {
        OnExperienceChanged?.Invoke(currentExperience);
    }

    public event Action<Abilities> OnAbilityAcquired;

    public void AbilityAcquired(Abilities ability)
    {
        OnAbilityAcquired?.Invoke(ability);
    }

    public event Action OnGhostSpeakActivated;

    // refer to this event when player gets Ghost Speak so all the ghosts in the game are enabled
    public void GhostSpeakActivated()
    {
        OnGhostSpeakActivated?.Invoke();
    }
}
