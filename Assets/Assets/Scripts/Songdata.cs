using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Songdata : MonoBehaviour
{
    public float bpm;
    public float crotchet;
    public float songPosition;
    public float deltaSongPos;
    public float lastHit;
    public float actialLastHit;
    float nextBeatTime = 0.0f;
    float nextBarTime = 0.0f;
    public float offset = 0.2f;
    public float addOffset;
    public static float staticOffset = 0.40f;
    public static bool hasOffsetAdjusted = false;
    public int beatNumber = 0;
    public int barNumber = 0;
    public AudioSource song;

    private void Update()
    {
        songPosition = song.time;
        deltaSongPos = songPosition * 1000;
        crotchet = 60f / bpm;
    }


}
