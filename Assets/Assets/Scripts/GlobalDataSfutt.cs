using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;
using System;

public static class GlobalDataSfutt
{
    public static string songNameToLoad = "DefaultSusName";
    public static int selectedStage = 0;
    public static bool ghostTapping, overrideStage, hasLoadedMods;
    public static string[] stages = new string[]
    {
        "stage",
        "spooky",
        "alley",
        "car",
        "mall",
        "school",
        "school-evil"
    };

    public static List<FNFCharacter> customCharacters = new List<FNFCharacter>();
    public static List<bool> characterActive = new List<bool>();

    public static bool isStoryMode;
    public static string[] weekSongs;
    public static int currentWeekSong;

    public static void LoadNextStoryModeSong()
    {
        currentWeekSong++;
        if (currentWeekSong < weekSongs.Length)
        {
            songNameToLoad = weekSongs[currentWeekSong];
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            isStoryMode = false;
            GoToMainMenu();
        }
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static string ReadShit(string songName)
    {
        string path = GetFilePath(songName);
        Debug.Log("trying to find shit at " + path);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                Debug.Log("Loaded shit succesfully, the extracted json is " + json);
                return json;
            }
        }
        else
        {
            Debug.LogError("Shit not found, u sure it even exists?");
            return "";
        }
    }

    public static string GetFilePath(string songName)
    {
        if (songNameToLoad != "" || songNameToLoad != null)
        {
            return Application.persistentDataPath + "/" + songNameToLoad;
        }
        else
        {
            return Application.persistentDataPath + "/" + songName + ".json";
        }
    }

    public static T ImportXml<T>(string path)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception importing xml file: " + e);
            return default;
        }
    }
}
