using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class CreateSectionLeft : MonoBehaviour
{

    public List<GameObject> TogglesObject;
    public List<bool> Values;
    public List<float> noteTime;
    public List<int> HoldTime;
    [SerializeField] SongJson song = new SongJson();

    Exported exported;

    public float crocket;
    public int SectionId = 0;
    public SectionId id;
    
    //create a list of the toggles, kinda useless. just needed fot the update list method
    void Start()
    {
        //string json = GlobalDataSfutt.ReadShit(GlobalDataSfutt.songNameToLoad);
        //JsonUtility.FromJsonOverwrite(json, song);

        SectionId = id.currentSectionId;


        foreach (Transform child in transform)
        { 
            TogglesObject.Add(child.gameObject);
        }
        //StartCoroutine(DoTheLoading());
        
    }
    //updates chart list with booleans, gonna change this soon
    public void UpdateList()
    {
        Values.Clear();
        noteTime.Clear();
        HoldTime.Clear();

        foreach (GameObject Value in TogglesObject)
        {  
            Toggle thing;

            thing = Value.GetComponent<Toggle>();
            Values.Add(thing.isOn);
            HoldTime.Add(thing.GetComponent<ToggleHoldNote>().HoldNoteTime);
        }
        for (int i = 0; i < 16; i++)
        {
            if (Values[i] == true)
            {
                noteTime.Add(((150 * i) + 2400 * SectionId) * -1);
            }
            else
            {
                noteTime.Add(0);
            }
        }

        if (Values[0] == true && SectionId == 0)
        {
            noteTime[0] = 0.1f;
        }
    }

    IEnumerator DoTheLoading()
    {
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < 16; i++)
        {
            Toggle thing;
            GameObject stuff;
            int coolName;
            coolName = i + (16 * SectionId);
            stuff = TogglesObject.ElementAt(i);
            thing = stuff.GetComponent<Toggle>();
            //thing.isOn = song.notesLeft[coolName];
            if (song.notesLeft[coolName] != 0)
            {
                thing.isOn = true;
            }
            else
            {
                thing.isOn = false;
            }
        }
        crocket = (60000f / song.bpm) / 16;
        UpdateList();
    }

    public void ClearSection()
    {
        for (int i = 0; i < 16; i++)
        {
            Toggle thing;
            GameObject stuff;
            int coolName;
            coolName = i + (16 * SectionId);
            stuff = TogglesObject.ElementAt(i);
            thing = stuff.GetComponent<Toggle>();
            thing.isOn = false;
        }
        UpdateList();
    }

}

[System.Serializable]
public class SongJson
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