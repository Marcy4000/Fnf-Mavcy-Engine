using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoUp : MonoBehaviour
{
    //[HideInInspector]public LoadSong song;
    [HideInInspector]public RectTransform noteTransform;
    [HideInInspector]public Countdown countdown;
    private Animator noteAnimator;
    private float speed;
    private Transform thisTransform;
    double lastBeat;

    // Start is called before the first frame update
    void Start()
    {
        GameObject songLoader;
        GameObject counttdown;
        songLoader = GameObject.Find("SongLoader");
        //song = songLoader.GetComponent<LoadSong>();
        noteTransform = this.GetComponent<RectTransform>();
        counttdown = GameObject.Find("3-2-1-Go");
        countdown = counttdown.GetComponent<Countdown>();
        noteAnimator = this.GetComponent<Animator>();
        //noteAnimator.speed = (1f * song.scrollSpeed) / Songdata.stepCrotchet;
        //speed = (1f * song.scrollSpeed) / Songdata.stepCrotchet;
        speed = Songdata.bpm / 60;
        thisTransform = this.transform;
    }

    private void Update()
    {
        if (countdown.hasCountDownFinished)
        {
            /*transform.Translate(new Vector3(0, ((Vector3.up.y * Time.deltaTime) * song.scrollSpeed) * 1000, 0));
            if (Songdata.songPosition > lastBeat + Songdata.stepCrotchet / 2)
            {
                transform.Translate(new Vector3(0, 50f * song.scrollSpeed, 0));
                StartCoroutine(ChangeSomeValue(transform.position.y, transform.position.y + 75, Songdata.stepCrotchet / 2));
                lastBeat += Songdata.stepCrotchet / 2;
            }
            StartCoroutine(MoveShit());

            transform.position += new Vector3(0f, (speed * 700) * Time.deltaTime, 0f);*/

            transform.DOMoveY(transform.position.y + 10, (float)Songdata.stepCrotchet / 2);

        }

    }
    
    //Lmao copied from stack overflow
    public IEnumerator ChangeSomeValue(float oldValue, float newValue, double duration)
    {
        for (double t = 0f; t < duration; t += Time.deltaTime / Songdata.crotchet)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(oldValue, newValue, (float)(t / duration)), transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, newValue, transform.position.z);
    }
}
