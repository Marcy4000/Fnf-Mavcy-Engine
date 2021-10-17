using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public GoUp notes;
    public LoadSong songLoader;

    public bool hasCountDownFinished;
    public bool hasCoundDownStarted;

    public AudioSource three;
    public AudioSource two;
    public AudioSource one;
    public AudioSource go;

    public Animator countdown;

    private void OnEnable()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        hasCoundDownStarted = true;
        countdown.SetBool("IsActive", true);
        three.Play();
        yield return new WaitForSeconds(Songdata.crotchet);

        two.Play();
        countdown.SetTrigger("Ready?");

        yield return new WaitForSeconds(Songdata.crotchet);

        one.Play();
        countdown.SetTrigger("Set");

        yield return new WaitForSeconds(Songdata.crotchet);

        go.Play();
        countdown.SetTrigger("Go");
        hasCountDownFinished = true;
        countdown.SetBool("IsActive", false);
        songLoader.inst.Play();
        songLoader.voices.Play();
    }
}
