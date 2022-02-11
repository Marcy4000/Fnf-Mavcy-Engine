using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactOnBeat : MonoBehaviour
{
    Animator animator;
    public string animationName;
    public int beatnumber = 4;
    int currentBeat;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
        currentBeat++;
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
        }
    }
}
