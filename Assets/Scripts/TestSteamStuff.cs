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
            Steamworks.SteamClient.Init(123456789);
        }
        catch (System.Exception e)
        {
            //something went wrong etc etc
            Debug.Log(e);
        }
    }
    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();

        if (Keyboard.current.gKey.wasPressedThisFrame && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            IsThisAchievementUnlocked("asd");
        }

        if (Keyboard.current.hKey.wasPressedThisFrame && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            UnlockAchievement("asd");
        }

        if (Keyboard.current.jKey.wasPressedThisFrame && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            ClearAchievementStatus("asd");
        }
    }

    private void OnApplicationQuit()
    {
        //it will show u are still playing the game on Steam until you close Unity
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
