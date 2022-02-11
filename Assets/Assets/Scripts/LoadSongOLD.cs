using UnityEngine;
using Bolt;
using System.IO;
using UnityEngine.UI;

public class LoadSongOLD : MonoBehaviour
{
    [HideInInspector] public GameObject scene;

    public BfAnimations bf;

    public int bpm;
    public float scrollSpeed;
    public string songName = "Giuseppe";
    //public Songdata songdata;

    [SerializeField] Exported song = new Exported();

    public GameObject leftNote;
    public GameObject DownNote;
    public GameObject UpNote;
    public GameObject RightNote;
    //public GameObject Space;

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

    public NoteCheckThing leftChecker, downChecker, upChecker, rightChecker; 

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
        Songdata.bpm = song.bpm;
        scrollSpeed = song.scrollSpeed;
        bpm = song.bpm;

        try
        {
            inst.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3"));
            voices.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Voices.mp3"));
            Debug.Log(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3");

        }
        catch (System.Exception)
        {
            Debug.Log(Application.persistentDataPath + "/Songs/" + song.songName + "/" + "Inst.mp3" + "file path does not exist u idiot, going to main menu");
            GlobalDataSfutt.GoToMainMenu();
        }

        if (Songdata.bpm != 0)
        {
            //bf.crocket = 60 / Songdata.bpm;
        }
        Songdata.Initialize();

        for (int i = 0; i < song.notesLeft.Length; i++)//Left
        {
            if (song.notesLeft[i] != 0)
            {
                GameObject currentNote = Object.Instantiate(leftNote, new Vector3(1243, (song.notesLeft[i]) - 100, 0), Quaternion.identity, LeftSection.transform);
                leftChecker.notesToCheck.Add((song.notesLeft[i] / 150 * -1));
                leftChecker.notes.Add(currentNote);
                if (leftChecker.notesToCheck[0] > -0.1f && leftChecker.notesToCheck[0] < 0.1f || leftChecker.notesToCheck[0] == 0.0006666667f)
                {
                    leftChecker.notesToCheck[0] = 0;
                    leftChecker.currentNoteToCheck = 0;
                }
                if (song.holdNotesLeft[i] > 0)
                {
                    currentNote.GetComponent<Note>().isHoldNote = true;
                    currentNote.GetComponent<Note>().holdTime = song.holdNotesLeft[i] * (int)scrollSpeed;
                }
            }

        }
        for (int l = 0; l < song.notesDown.Length; l++)//down
        {
            if (song.notesDown[l] != 0)
            {
                GameObject currentNote = Object.Instantiate(DownNote, new Vector3(1393, (song.notesDown[l]) - 100, 0), Quaternion.identity, DownSection.transform);
                downChecker.notesToCheck.Add((song.notesDown[l] / 150 * -1));
                downChecker.notes.Add(currentNote);
                if (downChecker.notesToCheck[0] > -0.1f && downChecker.notesToCheck[0] < 0.1f || downChecker.notesToCheck[0] == 0.0006666667f)
                {
                    downChecker.notesToCheck[0] = 0;
                    downChecker.currentNoteToCheck = 0;
                }
                if (song.holdNotesDown[l] > 0)
                {
                    currentNote.GetComponent<Note>().isHoldNote = true;
                    currentNote.GetComponent<Note>().holdTime = song.holdNotesDown[l];
                }
            }

        }
        for (int k = 0; k < song.notesUp.Length; k++)//Up
        {
            if (song.notesUp[k] != 0)
            {
                GameObject currentNote = Object.Instantiate(UpNote, new Vector3(1543, (song.notesUp[k]) - 100, 0), Quaternion.identity, UpSection.transform);
                upChecker.notesToCheck.Add((song.notesUp[k] / 150 * -1));
                upChecker.notes.Add(currentNote);
                if (upChecker.notesToCheck[0] > -0.1f && upChecker.notesToCheck[0] < 0.1f || upChecker.notesToCheck[0] == 0.0006666667f)
                {
                    upChecker.notesToCheck[0] = 0;
                    upChecker.currentNoteToCheck = 0;
                }
                if (song.holdNotesUp[k] > 0)
                {
                    currentNote.GetComponent<Note>().isHoldNote = true;
                    currentNote.GetComponent<Note>().holdTime = song.holdNotesUp[k];
                }
            }

        }
        for (int q = 0; q < song.notesRight.Length; q++)//Right
        {
            if (song.notesRight[q] != 0)
            {
                GameObject currentNote = Object.Instantiate(RightNote, new Vector3(1693, (song.notesRight[q])- 100, 0), Quaternion.identity, RightSection.transform);
                rightChecker.notesToCheck.Add((song.notesRight[q] / 150 * -1));
                rightChecker.notes.Add(currentNote);
                if (rightChecker.notesToCheck[0] > -0.1f && rightChecker.notesToCheck[0] < 0.1f || rightChecker.notesToCheck[0] == 0.0006666667f)
                {
                    rightChecker.notesToCheck[0] = 0;
                    rightChecker.currentNoteToCheck = 0;
                }
                if (song.holdNotesRight[q] > 0)
                {
                    currentNote.GetComponent<Note>().isHoldNote = true;
                    currentNote.GetComponent<Note>().holdTime = song.holdNotesRight[q];
                }
            }

        }
        leftChecker.currentNoteToCheck = leftChecker.notesToCheck[0];
        leftChecker.nextNoteToCheck = leftChecker.notesToCheck[1];
        
        downChecker.currentNoteToCheck = downChecker.notesToCheck[0];
        downChecker.nextNoteToCheck = downChecker.notesToCheck[1];
        
        upChecker.currentNoteToCheck = upChecker.notesToCheck[0];
        upChecker.nextNoteToCheck = upChecker.notesToCheck[1];
        
        rightChecker.currentNoteToCheck = rightChecker.notesToCheck[0];
        rightChecker.nextNoteToCheck = rightChecker.notesToCheck[1];

        
    }

    private void Update()
    {
        Songdata.SetSongTime(inst);
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

    //probally i should not use booleans, but i'm too lazy to change
    //Changed from booleans :3
    public float[] notesLeft;
    public float[] notesDown;
    public float[] notesUp;
    public float[] notesRight;

    public int[] holdNotesLeft;
    public int[] holdNotesDown;
    public int[] holdNotesUp;
    public int[] holdNotesRight;
}