using System;
using static PlayerManager;

public class PlayerEvents
{
    public event Action<int> onExperienceGained;

    // refer to this event when GETTING new experience for example after completing a quest
    public void ExperienceGained(int newExperience)
    {
        if (onExperienceGained != null)
        {
            onExperienceGained(newExperience);
        }
    }

    public event Action<int> onExperienceChanged;

    // refer to this event when experience HAS BEEN UPDATED and the results need to be shown for example in UI
    public void ExperienceChanged(int currentExperience)
    {
        if (onExperienceChanged != null)
        {
            onExperienceChanged(currentExperience);
        }
    }
    public event Action<int> onAbilityGet;
    public void AbilityGet(int index)
    {
        if (onAbilityGet != null)
        {
            onAbilityGet(index);
        }
    }

    public event Action onGhostSpeakActivated;

    // refer to this event when player gets Ghost Speak so all the ghosts in the game are enabled
    public void GhostSpeakActivated()
    {
        if (onGhostSpeakActivated != null)
        {
            onGhostSpeakActivated();
        }
    }
}
