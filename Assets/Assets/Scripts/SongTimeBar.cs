using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SongTimeBar : MonoBehaviour
{
    public TMP_Text timeText;
    public Slider timeBar;

    void Update()
    {
        if (timeBar.maxValue != LoadSong.instance.inst.clip.length)
        {
            timeBar.maxValue = LoadSong.instance.inst.clip.length;
        }
        if (!LoadSong.instance.songStarted)
            return;

        TimeSpan ts = TimeSpan.FromSeconds(LoadSong.instance.inst.clip.length) - TimeSpan.FromSeconds(Songdata.songPosition);
        timeBar.value = Songdata.songPosition;
        timeText.text = $"{ts.Minutes}:{ts.Seconds}";
        
    }
}
