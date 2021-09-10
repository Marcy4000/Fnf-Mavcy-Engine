using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExportSong : MonoBehaviour
{
    [HideInInspector]public SectionMenager menager;


    public TMP_InputField songName;
    public TMP_InputField bpm;
    public TMP_InputField scrollSpeed;
    [SerializeField] Exporting song = new Exporting(); //idk what this is but it works so...
    public GameObject[] leftSection;
    public GameObject[] downSection;
    public GameObject[] upSection;
    public GameObject[] rightSection;

    public List<bool> leftFullChart;
    public List<bool> downFullChart;
    public List<bool> upFullChart;
    public List<bool> rightFullChart;


    void Start()
    {
        /*
        //getting note list from section (old)
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
        */
        GameObject manager = GameObject.Find("SectionMenu");
        menager = manager.GetComponent<SectionMenager>();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(1);
        }
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
        /*
         * song.notesLeft = noteListLeft.Values.ToArray();
        song.notesDown = noteListDown.Values.ToArray();
        song.notesUp = noteListUp.Values.ToArray();
        song.notesRight = noteListRight.Values.ToArray();
        */
        for (int i = 0; i < menager.highestSectionIdSelected + 1; i++)
        {
            CreateSection section;
            List<bool> currentChart;
            section = leftSection[i].GetComponent<CreateSection>();
            
            currentChart = section.Values;
            //leftFullChart.AddRange(currentChart);
            currentChart.ForEach(item => leftFullChart.Add(item));
        }
        for (int l = 0; l < menager.highestSectionIdSelected + 1; l++)
        {
            CreateSection section;
            List<bool> currentChart;
            section = downSection[l].GetComponent<CreateSection>();

            currentChart = section.Values;
            //downFullChart.AddRange(currentChart);
            currentChart.ForEach(item => downFullChart.Add(item));
        }
        for (int k = 0; k < menager.highestSectionIdSelected + 1; k++)
        {
            CreateSection section;
            List<bool> currentChart;
            section = upSection[k].GetComponent<CreateSection>();

            currentChart = section.Values;
            //upFullChart.AddRange(currentChart);
            currentChart.ForEach(item => upFullChart.Add(item));
        }
        for (int s = 0; s < menager.highestSectionIdSelected + 1; s++)
        {
            CreateSection section;
            List<bool> currentChart;
            section = rightSection[s].GetComponent<CreateSection>();

            currentChart = section.Values;
            //rightFullChart.AddRange(currentChart);
            currentChart.ForEach(item => rightFullChart.Add(item));

        }
        song.notesLeft = leftFullChart.ToArray();
        song.notesDown = downFullChart.ToArray();
        song.notesUp = upFullChart.ToArray();
        song.notesRight = rightFullChart.ToArray();

        //if song name is empty just default to name
        if (song.bpm == 0)
        {
            song.bpm = 100;
        }
        if (song.songName == null || song.songName == "")
        {
            song.songName = "DefaultSusName";
        }

        //idk what this shit does, just googled it lol
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
    public static T[] Unify<T>(this T[] first, T[] second)
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