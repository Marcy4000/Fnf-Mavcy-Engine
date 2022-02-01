using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BfAnimations : MonoBehaviour
{
    public bool ShouldMove = true;
    Animator bfAnimator;
    public float crocket;
    public int beatnumber = 4;
    double lastBeat;
    // Start is called before the first frame update
    void Start()
    {
        bfAnimator = this.GetComponent<Animator>();
        lastBeat = 0;
    }

    private void OnEnable()
    {
        lastBeat = 0;
    }

    private void Update()
    {
        if (Songdata.songPosition > lastBeat + Songdata.crotchet)
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

            lastBeat += Songdata.crotchet;
        }
    }

    public void BoyfriendPlayAnimation(string animationName)
    {
        bfAnimator.Play("BF " + animationName, 0, 0);
        bfAnimator.speed = 0;

        bfAnimator.Play("BF " + animationName);
        bfAnimator.speed = 1;

        StopAllCoroutines();
        StartCoroutine(DumbThing());

    }

    IEnumerator DumbThing()
    {
        yield return new WaitForSeconds(0.4f);
        
        bfAnimator.Play("Bf-Idle-static");
    }
}
