using System;

public class PlayerEvents
{
    public event Action<int> onExperienceGained;

    // refer to this event when GETTING new experience for example after completing a quest
    public void ExperienceGained(int newExperience)
    {
        if(onExperienceGained != null)
        {
            onExperienceGained(newExperience);
        }
    }

    public event Action<int> onExperienceChanged;

    // refer to this event when experience HAS BEEN UPDATED and the results need to be shown for example in UI
    public void ExperienceChanged(int currentExperience)
    {
        if(onExperienceChanged != null)
        {
            onExperienceChanged(currentExperience);
        }
    }
}
