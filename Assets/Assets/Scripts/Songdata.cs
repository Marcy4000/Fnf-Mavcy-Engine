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
    public static int currSection = 0;
    private static bool changeSection = false;

    public delegate void BeatEvent();
    public static event BeatEvent OnBeat;
    public delegate void SectionEvent();
    public static event SectionEvent OnChangeSection;

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
        changeSection = false;
        currSection = 0;
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
            changeSection = true;
            lastHitB += stepCrotchet;
        }

        if (barNumber % 16 == 0 && changeSection)
        {
            currSection++;
            OnChangeSection?.Invoke();
            changeSection = false;
        }
    }

}
