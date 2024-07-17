using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSteamStuff : MonoBehaviour
{
    void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(2700090);
        }
        catch (System.Exception e)
        {
            //something went wrong etc etc
            Debug.LogError(e);
        }
    }
    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            IsThisAchievementUnlocked("ACH_TEST");
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            UnlockAchievement("ACH_TEST");
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ClearAchievementStatus("ACH_TEST");
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            Steamworks.SteamClient.Shutdown();
        }
    }

    private void OnApplicationQuit()
    {
        //it will show u are still playing the game on Steam until you close Unity
        //unless this runs, you'll have a change stuck in Github that you cant discard
        Steamworks.SteamClient.Shutdown();
    }

    public void IsThisAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);

        Debug.Log($"Achievement {id} status: {ach.State}");
    }

    public void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();

        Debug.Log($"Achievement {id} unlocked");
    }

    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();

        Debug.Log($"Achievement {id} cleared");
    }
}
