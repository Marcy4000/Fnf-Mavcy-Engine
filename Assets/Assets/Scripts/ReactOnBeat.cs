using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactOnBeat : MonoBehaviour
{
    Animator animator;
    public string animationName;
    public int beatnumber = 4;
    //int currentBeat;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (beatnumber == 0)
        {
            beatnumber = 1;
        }
    }

    private void OnEnable()
    {
        Songdata.OnBeat += BeatStuff;
    }

    private void OnDisable()
    {
        Songdata.OnBeat -= BeatStuff;
    }


    private void BeatStuff()
    {
        if (Songdata.beatNumber % beatnumber == 0)
        {
            animator.Play(animationName);
        }
        
        
        /*currentBeat++;
        if (currentBeat > 3)
        {
            currentBeat = 0;
        }
        switch (beatnumber)
        {
            case 0:
                animator.Play(animationName);
                break;
            case 1:
                if (currentBeat == 0)
                {
                    animator.Play(animationName);
                }
                break;
            case 2:
                if (currentBeat == 0 || currentBeat == 2)
                {
                    animator.Play(animationName);
                }
                break;
        }*/
    }
}
