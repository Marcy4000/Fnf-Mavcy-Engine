using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BfAnimations : MonoBehaviour
{
    public bool ShouldMove = true;
    Animator bfAnimator;
    public float crocket;
    public int beatnumber = 4;
    float lastBeat;
    // Start is called before the first frame update
    void Start()
    {
        bfAnimator = this.GetComponent<Animator>();
        lastBeat = 0;
    }

    private void Update()
    {
        if (Songdata.songPosition > lastBeat + Songdata.crotchet)
        {
            if (!bfAnimator.GetBool("Singing"))
            {
                switch (ShouldMove)
                {
                    case true:
                        bfAnimator.SetBool("ShouldMove", false);
                        ShouldMove = false;
                        break;
                    case false:
                        bfAnimator.SetBool("ShouldMove", true);
                        ShouldMove = true;
                        break;
                }
            }

            lastBeat += Songdata.crotchet;
        }
    }
}
