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
    private bool isCustomChar;
    private int currentChar;
    private SpriteRenderer spriteRenderer;
    private bool singing = false;
    // Start is called before the first frame update

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastBeat = 0;
    }

    public void Initialize(bool customChar, int _currentChar)
    {
        if (customChar)
        {
            enemyAnimator.enabled = false;
            isCustomChar = true;
            currentChar = _currentChar;
        }
        else
        {
            enemyAnimator.enabled = true;
            isCustomChar = false;
        }
    }

    private void OnEnable()
    {
        lastBeat = 0;
    }

    private void Update()
    {
        if (Songdata.songPosition > lastBeat + Songdata.crotchet)
        {
            if (!isCustomChar)
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
            }
            else
            {
                switch (ShouldMove)
                {
                    case true:
                        ShouldMove = false;
                        break;
                    case false:
                        if (!singing)
                            AnimationSystem.instance.Play(spriteRenderer, "idle", currentChar, false);
                        ShouldMove = true;
                        break;
                }
            }

            lastBeat += Songdata.crotchet;
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (!isCustomChar)
        {
            enemyAnimator.Play("Enemy " + animationName, 0, 0);
            enemyAnimator.speed = 0;

            enemyAnimator.Play("Enemy " + animationName);
            enemyAnimator.speed = 1;
        }
        else
        {
            singing = true;
            AnimationSystem.instance.Play(spriteRenderer, animationName, currentChar, false);
        }

        StopAllCoroutines();
        StartCoroutine(DumbThing());

    }

    IEnumerator DumbThing()
    {
        yield return new WaitForSeconds(0.4f);

        if (!isCustomChar)
        {
            enemyAnimator.Play("Idle-Static");
        }
        else
        {
            singing = false;
            AnimationSystem.instance.Play(spriteRenderer, "idle", currentChar, false);
        }
    }
}
