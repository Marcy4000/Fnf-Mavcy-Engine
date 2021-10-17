using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GfBop : MonoBehaviour
{
    public bool isLeft = false;
    Animator gfAnimator;
    public float crocket;
    public int beatnumber = 4;
    float lastBeat;
    // Start is called before the first frame update
    void Start()
    {
        gfAnimator = this.GetComponent<Animator>();
        lastBeat = 0;
        if (Songdata.bpm != 0)
        {
            crocket = 60 / Songdata.bpm;
        }
    }

    private void Update()
    {
        if (Songdata.songPosition > lastBeat + crocket)
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

            lastBeat += crocket;
        }
    }
}

