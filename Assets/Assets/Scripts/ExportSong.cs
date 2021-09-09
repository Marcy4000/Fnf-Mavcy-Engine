using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportSong : MonoBehaviour
{
    CreateSection noteListLeft;
    CreateSection noteListDown;
    CreateSection noteListUp;
    CreateSection noteListRight;

    public TMP_InputField songName;
    public TMP_InputField bpm;
    public TMP_InputField scrollSpeed;
    [SerializeField] Exporting song = new Exporting(); //idk what this is but it works so...
    

    void Start()
    {
        //getting note list from section
        GameObject sectionLeft = GameObject.Find("SectionLeft");//Left
        noteListLeft = sectionLeft.GetComponent<CreateSection>();
        //down
        GameObject sectionDown = GameObject.Find("SectionDown");
        noteListDown = sectionDown.GetComponent<CreateSection>();
        //Up
        GameObject sectionUp = GameObject.Find("SectionUp");
        noteListUp = sectionUp.GetComponent<CreateSection>();
        //Right
        GameObject sectionRight = GameObject.Find("SectionRight");
        noteListRight = sectionRight.GetComponent<CreateSection>();
    }

    
    public void Export()
    {
        //making sure the player doesn't just change shit while saving
        songName.readOnly = true;
        bpm.readOnly = true;
        scrollSpeed.readOnly = true;
        
        //Setting correct values to save
        song.songName = songName.text;
        song.bpm = int.Parse(bpm.text);
        song.scrollSpeed = float.Parse(scrollSpeed.text);
        song.notesLeft = noteListLeft.Values.ToArray();
        song.notesDown = noteListDown.Values.ToArray();
        song.notesUp = noteListUp.Values.ToArray();
        song.notesRight = noteListRight.Values.ToArray();
        //if song name is empty just default to name
        if (song.bpm == 0)
        {
            song.bpm = 100;
        }
        if (song.songName == null || song.songName == "")
        {
            song.songName = "DefaultSusName";
        }

        //idk what this shit does, just googled it
        string chart = JsonUtility.ToJson(song);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + song.songName + ".json", chart);
        Debug.Log("this shit was saved in " + Application.persistentDataPath);

        songName.readOnly = false;
        bpm.readOnly = false;
        scrollSpeed.readOnly = false;
    }
}

[System.Serializable]
public class Exporting
{
    //json values
    public string songName = "DefaultSusName";
    public int bpm = 150;
    public float scrollSpeed = 7f;
    
    //probally i should not use booleans, but i'm too lazy to change
    public bool[] notesLeft;
    public bool[] notesDown;
    public bool[] notesUp;
    public bool[] notesRight;
}

public static class Extension
{
    public static T[] Concatenate<T>(this T[] first, T[] second)
    {
        if (first == null)
        {
            return second;
        }
        if (second == null)
        {
            return first;
        }

        return first.Concat(second).ToArray();
    }
}