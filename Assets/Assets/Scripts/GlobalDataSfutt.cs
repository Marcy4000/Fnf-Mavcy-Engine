using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class GlobalDataSfutt
{
    public static string songNameToLoad = "DefaultSusName";
    public static int selectedStage = 0;
    public static bool ghostTapping;
    public static string[] stages = new string[]
    {
        "stage",
        "spooky",
        "alley",
        "car",
        "mall",
        "school"
    };

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
}
