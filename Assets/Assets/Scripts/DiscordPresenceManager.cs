using System;
using UnityEngine;
using Discord;

public class DiscordPresenceManager : MonoBehaviour
{
    public static DiscordPresenceManager instance;
    public static string clientID;
    public Discord.Discord discord;
    public ApplicationManager applicationManager;
    public UserManager userManager;
    public bool hasSetup = false;
    
    private void Start()
    {
        DiscordPresenceManager[] things = FindObjectsOfType<DiscordPresenceManager>();
        if (things.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        instance = this;
        
        clientID = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
        if (clientID == null)
        {
            clientID = "944962652990480385";
        }
        discord = new Discord.Discord(Int64.Parse(clientID), (UInt64)CreateFlags.Default);
        discord.SetLogHook(LogLevel.Debug, (level, message) =>
        {
            Debug.Log($"Log[{level}] {message}");
        });
        applicationManager = discord.GetApplicationManager();
        // Get the current locale. This can be used to determine what text or audio the user wants.
        Debug.Log($"Current Locale: {applicationManager.GetCurrentLocale()}");
        // Get the current branch. For example alpha or beta.
        Debug.Log($"Current Branch: {applicationManager.GetCurrentBranch()}");

        userManager = discord.GetUserManager();
        userManager.OnCurrentUserUpdate += () =>
        {
            var currentUser = userManager.GetCurrentUser();
            Debug.Log(currentUser.Username);
            Debug.Log(currentUser.Id);
        };
        UpdateActivity(new Activity
        {
            Name = "Fnf Mavcy Engine",
            State = "In the menus",
            Assets =
            {
                LargeImage = "logo",
                LargeText = "logo",
            },
            Instance = true,
        });
        hasSetup = true;
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }

    private void Update()
    {
        discord.RunCallbacks();
    }

    public static void UpdateActivity(Activity activity)
    {
        var _discord = new Discord.Discord(Int64.Parse(clientID), (UInt64)CreateFlags.Default);
        _discord.SetLogHook(LogLevel.Debug, (level, message) =>
        {
            Debug.Log($"Log[{level}] {message}");
        });
        var activityManager = _discord.GetActivityManager();
        activityManager.UpdateActivity(activity, result =>
        {
            if (result == Result.Ok)
            {
                Debug.Log("Did the thing");
            }
            else
            {
                Debug.Log("Did not the thing");
            }
        });
    }
}
