using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldArrow : MonoBehaviour
{
    [HideInInspector]public GameObject leftChecker, downChecker, upChecker, rightChecker;
    [HideInInspector]public Animator leftAnimator, downAnimator, upAnimator, rightAnimator;
    public GameObject canavas;
    public int currentNoteDirection;

    private void Start()
    {
        leftChecker = GameObject.Find("LeftDetector");
        downChecker = GameObject.Find("DownDetector");
        upChecker = GameObject.Find("UpDetector");
        rightChecker = GameObject.Find("RightDetector");
        canavas = GameObject.Find("Canvas");

        leftAnimator = leftChecker.GetComponent<Animator>();
        downAnimator = downChecker.GetComponent<Animator>();
        upAnimator = upChecker.GetComponent<Animator>();
        rightAnimator = rightChecker.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentNoteDirection == 0)
        {
            if (collision.gameObject == leftChecker)
            {
                if (leftAnimator.GetFloat("Fuck") == 2)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
        if (currentNoteDirection == 1)
        {
            if (collision.gameObject == downChecker)
            {
                if (downAnimator.GetFloat("Fuck") == 2)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
        if (currentNoteDirection == 2)
        {
            if (collision.gameObject == upChecker)
            {
                if (upAnimator.GetFloat("Fuck") == 2)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
        if (currentNoteDirection == 3)
        {
            if (collision.gameObject == rightChecker)
            {
                if (rightAnimator.GetFloat("Fuck") == 2)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void DeChildObject()
    {
        this.transform.SetParent(canavas.transform, true);
    }
}
