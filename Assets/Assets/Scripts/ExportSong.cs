using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class ExportSong : MonoBehaviour
{
    public bool songLoaded = false;

    public TMP_InputField songName;
    public TMP_InputField bpm;
    public TMP_InputField scrollSpeed;

    public AudioSource inst, voices;
    public static float songTime;

    public TMP_Text songStats;

    void Start()
    {
        StartCoroutine(LoadSongMusic());
        Songdata.Initialize(Songdata.bpm);
    }

    IEnumerator LoadSongMusic()
    {
        string url = UnityWebRequest.EscapeURL(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad);
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + url + @"\Inst.ogg", AudioType.OGGVORBIS);
        yield return req.SendWebRequest();
        inst.clip = DownloadHandlerAudioClip.GetContent(req);
        while (inst.clip.loadState != AudioDataLoadState.Loaded)
            yield return new WaitForSeconds(0.1f);

        if (File.Exists(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad + @"\Voices.ogg"))
        {
            UnityWebRequest req2 = UnityWebRequestMultimedia.GetAudioClip("file:///" + url + @"\Voices.ogg", AudioType.OGGVORBIS);
            yield return req2.SendWebRequest();
            voices.clip = DownloadHandlerAudioClip.GetContent(req2);
            while (voices.clip.loadState != AudioDataLoadState.Loaded)
                yield return new WaitForSeconds(0.1f);
        }
        songLoaded = true;
    }

    private void Update()
    {
        //stuff for scene stuff, idk why i put those here
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        
        if (songLoaded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && inst.isPlaying == false)
            {
                inst.Play();
                voices.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && inst.isPlaying == true)
            {
                inst.Pause();
                voices.Pause();
            }
            Songdata.SetSongTime(inst);

            if (Input.mouseScrollDelta.y != 0)
            {
                inst.time -= Input.mouseScrollDelta.y / 10;
                voices.time = inst.time;
                if (Songdata.songPosition > inst.time)
                {
                    Songdata.Initialize(Songdata.bpm);
                }
            }

            songStats.text = $"Bpm:{Songdata.bpm}\nSong Time:{Math.Round(inst.time, 3)}/{inst.clip.length}\nBeat:{Songdata.beatNumber}\nBar:{Songdata.barNumber}\nSection:{Songdata.currSection}";
        }
        
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