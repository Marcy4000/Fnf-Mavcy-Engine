using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUp : MonoBehaviour
{
    [HideInInspector]public LoadSong song;
    [HideInInspector]public RectTransform noteTransform;
    [HideInInspector]public Countdown countdown;
    float lastBeat;

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

    private void Update()
    {
        if (countdown.hasCountDownFinished)
        {
            //transform.Translate(new Vector3(0, ((Vector3.up.y * Time.deltaTime) * song.scrollSpeed) * 1000, 0));
            if (Songdata.songPosition > lastBeat + Songdata.stepCrotchet)
            {
                //transform.Translate(new Vector3(0, 50f * song.scrollSpeed, 0));
                StartCoroutine(ChangeSomeValue(transform.position.y, transform.position.y + 150, Songdata.stepCrotchet));
                lastBeat += Songdata.stepCrotchet;
            }
        }
        
    }

    //Lmao copied from stack overflow
    public IEnumerator ChangeSomeValue(float oldValue, float newValue, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime / Songdata.crotchet)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(oldValue, newValue, t / duration), transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, newValue, transform.position.z);
    }
}
