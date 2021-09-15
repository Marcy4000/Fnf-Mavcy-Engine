using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUp : MonoBehaviour
{
    [HideInInspector]public LoadSong song;
    [HideInInspector]public RectTransform noteTransform;
    [HideInInspector]public Countdown countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject songLoader;
        GameObject counttdown;
        songLoader = GameObject.Find("SongLoader");
        song = songLoader.GetComponent<LoadSong>();
        noteTransform = this.GetComponent<RectTransform>();
        counttdown = GameObject.Find("3-2-1-Go");
        countdown = counttdown.GetComponent<Countdown>();
    }

    private void FixedUpdate()
    {
        if (countdown.hasCountDownFinished)
        {
            transform.Translate(new Vector3(0, ((Vector3.up.y * Time.deltaTime) * song.scrollSpeed) * 1000, 0));
        }
        
    }
}
