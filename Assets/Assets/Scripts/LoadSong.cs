using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.IO;
using UnityEngine.UI;
using NAudio;

public class LoadSong : MonoBehaviour
{
    [HideInInspector]public GameObject scene;

    public int bpm;
    public float scrollSpeed;
    public string songName = "Giuseppe";
    public Songdata songdata;
    
    [SerializeField] Exported song = new Exported();

    public GameObject leftNote;
    public GameObject DownNote;
    public GameObject UpNote;
    public GameObject RightNote;
    public GameObject Space;

    public GameObject LeftSection;
    public GameObject DownSection;
    public GameObject UpSection;
    public GameObject RightSection;

    public AudioSource inst;
    public AudioSource voices;

    GridLayoutGroup LeftThing;
    GridLayoutGroup DownThing;
    GridLayoutGroup UpThing;
    GridLayoutGroup RightThing;

    void Start()
    {
        GameObject scene = GameObject.Find("Scene Variables");
        LeftThing = LeftSection.GetComponent<GridLayoutGroup>();
        DownThing = DownSection.GetComponent<GridLayoutGroup>();
        UpThing = UpSection.GetComponent<GridLayoutGroup>();
        RightThing = RightSection.GetComponent<GridLayoutGroup>();

        string json = GlobalDataSfutt.ReadShit(songName);
        JsonUtility.FromJsonOverwrite(json, song);
        Debug.Log(song.songName);
        
        Variables.Scene(gameObject).Set("Bpm", song.bpm);
        Variables.Scene(gameObject).Set("ScrollSpeed", song.scrollSpeed);
        songdata.bpm = song.bpm;
        scrollSpeed = song.scrollSpeed;
        bpm = song.bpm;

        try
        {
            inst.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3"));
            voices.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(Application.persistentDataPath + "/Songs/" + song.songName + "/" +"Voices.mp3"));
            Debug.Log(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3");

        }
        catch (System.Exception)
        {
            Debug.Log(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3" +"file path does not exist u idiot, going to main menu");
            GlobalDataSfutt.GoToMainMenu();
        }


        for (int i = 0; i < song.notesLeft.Length; i++)//Left
        {
            if (song.notesLeft[i] == true)
            {
                Object.Instantiate(leftNote, LeftSection.transform);
            }
            else
            {
                Object.Instantiate(Space, LeftSection.transform);
            }
        }
        for (int l = 0; l < song.notesDown.Length; l++)//down
        {
            if (song.notesDown[l] == true)
            {
                Object.Instantiate(DownNote, DownSection.transform);
            }
            else
            {
                Object.Instantiate(Space, DownSection.transform);
            }
        }
        for (int k = 0; k < song.notesUp.Length; k++)//Up
        {
            if (song.notesUp[k] == true)
            {
                Object.Instantiate(UpNote, UpSection.transform);
            }
            else
            {
                Object.Instantiate(Space, UpSection.transform);
            }
        }
        for (int q = 0; q < song.notesRight.Length; q++)//Right
        {
            if (song.notesRight[q] == true)
            {
                Object.Instantiate(RightNote, RightSection.transform);
            }
            else
            {
                Object.Instantiate(Space, RightSection.transform);
            }
        }

       
    }

    public void DeactivateBitches()
    {
        //deactivating layout component, or notes are just going to move at random when destroyed
        LeftThing.spacing.Set(0, 10 * scrollSpeed);
        DownThing.spacing.Set(0, 10 * scrollSpeed);
        UpThing.spacing.Set(0, 10 * scrollSpeed);
        RightThing.spacing.Set(0, 10 * scrollSpeed);

        LeftThing.enabled = false;
        DownThing.enabled = false;
        UpThing.enabled = false;
        RightThing.enabled = false;
    }
}


[System.Serializable]
public class Exported
{
    //json values
    public string songName = "DefaultSusName";
    public int bpm = 150;
    public float scrollSpeed = 7f;

    public bool[] notesLeft;
    public bool[] notesDown;
    public bool[] notesUp;
    public bool[] notesRight;
}