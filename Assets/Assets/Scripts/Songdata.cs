using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Songdata
{
    //aka constructor
    public static float bpm;
    public static float crotchet;
    public static float stepCrotchet;
    public static float susStepCrotchet;
    public static float songPosition;
    public static float lastHit;
    public static float lastHitB;
    public static float offset = 0.2f;
    public static float addOffset;
    public static float staticOffset = 0.40f;
    public static bool hasOffsetAdjusted = false;
    public static int beatNumber = 0;
    public static int barNumber = 0;

    public delegate void BeatEvent();
    public static event BeatEvent OnBeat;

    public static void ResetThings()
    {
        //OnBeat = null;
    }
    
    public static void Initialize(float _bpm = 150)
    {
        bpm = _bpm;
        crotchet = 60 / bpm;
        stepCrotchet = crotchet / 4;
        lastHit = 0f;
        lastHitB = 0f;
        beatNumber = 0;
        barNumber = 0;
        songPosition = 0f;
        susStepCrotchet = 60 / bpm * 1000 / 4;
    }

    public static void ChangeBPM(float _bpm = 150)
    {
        bpm = _bpm;
        crotchet = 60 / bpm;
        stepCrotchet = crotchet / 4;
        susStepCrotchet = 60 / bpm * 1000 / 4;
    }
    
    public static void SetSongTime(AudioSource song)
    {
        songPosition = song.time;
        if (songPosition > lastHit + crotchet)
        {
            beatNumber++;
            OnBeat?.Invoke();
            lastHit += crotchet;
        }
        
        if (songPosition > lastHitB + stepCrotchet)
        {
            barNumber++;
            lastHitB += stepCrotchet;
        }
    }

}
