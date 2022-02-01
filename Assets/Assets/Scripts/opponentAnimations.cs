using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opponentAnimations : MonoBehaviour
{
    public bool ShouldMove = true;
    public Animator enemyAnimator;
    public float crocket;
    public int beatnumber = 4;
    double lastBeat;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = this.GetComponent<Animator>();
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
                    enemyAnimator.SetBool("ShouldMove", false);
                    ShouldMove = false;
                    break;
                case false:
                    enemyAnimator.SetBool("ShouldMove", true);
                    ShouldMove = true;
                    break;
            }

            lastBeat += Songdata.crotchet;
        }
    }

    public void PlayAnimation(string animationName)
    {
        enemyAnimator.Play("Enemy " + animationName, 0, 0);
        enemyAnimator.speed = 0;

        enemyAnimator.Play("Enemy " + animationName);
        enemyAnimator.speed = 1;

        StopAllCoroutines();
        StartCoroutine(DumbThing());

    }

    IEnumerator DumbThing()
    {
        yield return new WaitForSeconds(0.4f);

        enemyAnimator.Play("Idle-Static");
    }
}
