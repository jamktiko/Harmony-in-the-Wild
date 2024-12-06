using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteamManager : MonoBehaviour
{
    public static SteamManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Ability Manager.");
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
        void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(3385150);

            Debug.Log("successfully connected to Steam!");
            Debug.Log(Steamworks.SteamClient.AppId);
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
            IsThisAchievementUnlocked("BERRY_ACH");
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            UnlockAchievement("BERRY_ACH");
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ClearAchievementStatus("BERRY_ACH");
            ClearAchievementStatus("BERRY_ACH_2");
            ClearAchievementStatus("PINE_ACH_1");
            ClearAchievementStatus("PINE_ACH_2");
            ClearAchievementStatus("PINE_BERRY_ACH");
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            AchievementProgressBerry("stat_2");
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            AchievementProgressBerry("stat_3");
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

    public void AchievementProgressBerry(string id)
    {
        var stat = new Steamworks.Data.Stat(id);
        stat.Add(1);

        if (stat.GetInt() >= 10 && stat.GetInt() != 25)
        {
            UnlockAchievement("BERRY_ACH");
        }
        else if (stat.GetInt() == 25)
        {
            UnlockAchievement("BERRY_ACH_2");

            GetMasterGathererAchievement();
        }

        Debug.Log($"Achievement {id} progressed");
    }
    public void AchievementProgressPinecone(string id)
    {
        var stat = new Steamworks.Data.Stat(id);
        stat.Add(1);

        if (stat.GetInt() >=10&& stat.GetInt() != 25)
        {
            UnlockAchievement("PINE_ACH_1");
        }
        else if (stat.GetInt() == 25)
        {
            UnlockAchievement("PINE_ACH_2");

            GetMasterGathererAchievement();
        }

        Debug.Log($"Achievement {id} progressed");
    }

    public void GetMasterGathererAchievement() 
    {
        if (new Steamworks.Data.Stat("stat_2").GetInt()>=25&& new Steamworks.Data.Stat("stat_3").GetInt()>=25)
        {
            UnlockAchievement("PINE_BERRY_ACH");
        }
    }



    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();

        Debug.Log($"Achievement {id} cleared");
    }
}
