using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Songdata
{
    //aka constructor
    public static float bpm;
    public static float crotchet;
    public static float stepCrotchet;
    public static float songPosition;
    public static float lastHit;
    public static float lastHitB;
    public static float offset = 0.2f;
    public static float addOffset;
    public static float staticOffset = 0.40f;
    public static bool hasOffsetAdjusted = false;
    public static int beatNumber = 0;
    public static int barNumber = 0;

    public static void Initialize()
    {
        crotchet = 60 / bpm;
        stepCrotchet = crotchet / 4;
        lastHit = 0f;
        lastHitB = 0f;
        beatNumber = 0;
        barNumber = -8;
        songPosition = 0f;
    }
    
    public static void SetSongTime(AudioSource song)
    {
        songPosition = song.time;
        if (songPosition > lastHit + crotchet)
        {
            beatNumber++;
            lastHit += crotchet;
        }
        
        if (songPosition > lastHitB + stepCrotchet)
        {
            barNumber++;
            lastHitB += stepCrotchet;
        }
        //Debug.Log(barNumber + " " + beatNumber);
    }

}
