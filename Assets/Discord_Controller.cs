using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System.Data;
using System;
using UnityEngine.SceneManagement;

public class Discord_Controller : MonoBehaviour
{
    public long applicationID;
    [Space]
    public string details = "Exploring the world";
    public string state = "Being cool";
    [Space]
    public string largeImage = "game_logo";
    public string largeText = "Harmony in The Wild";

    Rigidbody rb;
    private long time;

    private static bool instanceExists;
    public Discord.Discord discord;


    private void Awake()
    {
        if (!instanceExists) 
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length>1) 
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    

    // Update is called once per frame
    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch 
        {

            Destroy(gameObject);
        }
    }
    private void LateUpdate()
    {
        UpdateStatus();
    }
    private void UpdateStatus()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state + SceneManager.GetActiveScene().name,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText,
                },
                Timestamps = 
                {
                    Start=time
                }
            };
            activityManager.UpdateActivity(activity, (res) => 
            {
                if (res!=Discord.Result.Ok)
                {
                    Debug.LogWarning("Failed to connect to Discord!");
                }
            });
        }
        catch 
        {

            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        discord.Dispose();
    }
}
