using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BfAnimations : MonoBehaviour
{
    public bool ShouldMove = true;
    Animator bfAnimator;

    void Start()
    {
        bfAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Songdata.OnBeat += BeatStuff;
    }

    private void OnDisable()
    {
        Songdata.OnBeat -= BeatStuff;
    }

    public void BeatStuff()
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
