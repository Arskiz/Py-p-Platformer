using System.Data;
using Discord;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Discord_Controller : MonoBehaviour
{
    public bool DebuggingGame;
    public long applicationID;
    [Space]
    public string details = "Debugging the game";
    public string distanceTraveled;
    public int itemsObtained;
    public string time_played;
    [Space]
    public string largeImage = "game_logo";
    public string largeText = "Pyöpä Platformer";
    private long time;

    public static bool instanceExists;
    public Discord.Discord discord;

    public Inventory inv;

    void Awake()
    {
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        inv = FindAnyObjectByType<Inventory>();
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
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

    void UpdateTimer()
    {
        //time_played = timeInSecondsTilLaunch.ToString();
    }

    void LateUpdate()
    {
        UpdateStatus();
        UpdateTimer();
    }

    void UpdateStatus()
    {
        try
        {
            if(DebuggingGame)
                details = "Debugging the world of Pyopä Platformer";
            else
                details = "Mahailee Pyöpä-maailmassa...";

            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                
                Details = details,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = time // Käytetään aloitusaikaa
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (discord != null)
        {
            Debug.Log("Disposing Discord instance.");
            discord.Dispose();
        }
    }
}
