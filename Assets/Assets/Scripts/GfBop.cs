using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GfBop : MonoBehaviour
{
    public bool isLeft = false;
    public Animator gfAnimator;

    void Start()
    {
        gfAnimator = GetComponent<Animator>();
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
        switch (isLeft)
        {
            case true:
                gfAnimator.SetBool("IsLeft", false);
                isLeft = false;
                break;
            case false:
                gfAnimator.SetBool("IsLeft", true);
                isLeft = true;
                break;
        }
    }
}

