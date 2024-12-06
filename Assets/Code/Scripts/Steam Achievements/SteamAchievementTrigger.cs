using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAchievementTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SteamManager.instance.UnlockAchievement("FINISH_ACH");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
