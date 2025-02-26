using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[System.Serializable]
public class DiscordController : MonoBehaviour
{
    [FormerlySerializedAs("applicationID")] public long ApplicationID;
    [FormerlySerializedAs("details")] [Space]
    public string Details = "Exploring the world";
    [FormerlySerializedAs("state")] public string State = "Being cool";
    [FormerlySerializedAs("largeImage")] [Space]
    public string LargeImage = "game_logo";
    [FormerlySerializedAs("largeText")] public string LargeText = "Harmony in The Wild";


    [FormerlySerializedAs("detailsValues")] [SerializeField]
    string[] _detailsValues;

    Rigidbody _rb;
    private long _time;

    private static bool _instanceExists;
    public Discord.Discord Discord;


    private void Awake()
    {
        if (!_instanceExists)
        {
            _instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Discord = new Discord.Discord(ApplicationID, (System.UInt64)global::Discord.CreateFlags.NoRequireDiscord);

        _time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }



    // Update is called once per frame
    void Update()
    {
        try
        {
            Discord.RunCallbacks();
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
            if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
            {
                Details = _detailsValues[2];
            }
            else if (SceneManager.GetActiveScene().name.Contains("OverWorld"))
            {
                Details = _detailsValues[0];
            }
            else if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                Details = _detailsValues[3];
            }
            else if (SceneManager.GetActiveScene().name.Contains("StoryBook"))
            {
                Details = _detailsValues[4];
            }
            else
            {
                Details = _detailsValues[1];
            }
            var activityManager = Discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = Details,
                //State = state + SceneManager.GetActiveScene().name,
                Assets =
                {
                    LargeImage = LargeImage,
                    LargeText = LargeText,
                },
                Timestamps =
                {
                    Start=_time
                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != global::Discord.Result.Ok)
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
        Discord.Dispose();
    }
}
