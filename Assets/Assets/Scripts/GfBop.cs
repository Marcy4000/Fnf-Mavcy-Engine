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
        crocket = Songdata.crotchet;
    }

    private void OnEnable()
    {
        lastBeat = 0;
    }

    private void Update()
    {
        if (Songdata.songPosition > lastBeat + Songdata.crotchet)
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

            lastBeat += Songdata.crotchet;
        }
    }
}

