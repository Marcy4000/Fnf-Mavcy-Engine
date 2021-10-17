using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCheckThing : MonoBehaviour
{
    public bool clicked;
    public bool colliding;
    
    public Animator arrowAnimator;
    public Animator bfAnimator;
    public KeyCode keyToCheck;
    public int bfSingValue;
    public List<GameObject> notes;
    public List<float> notesToCheck;
    public float currentNoteToCheck;
    public float nextNoteToCheck;
    public int currentIndex;


    public HealthBar healthBar;

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject note;
        note = collision.gameObject;

        if (clicked)
        {
            StartCoroutine(DestroyNote(note));
        }

    }*/

    private void Update()
    {
        if (Input.GetKeyDown(keyToCheck))
        {
            StartCoroutine(SetClicked());
        }
        if (Input.GetKeyUp(keyToCheck))
        {
            arrowAnimator.SetFloat("Fuck", 0);
            bfAnimator.SetBool("Singing", false);
        }
        if (Input.GetKey(keyToCheck))
        {
            if (clicked)
            {
                if (!colliding)
                {
                    arrowAnimator.SetFloat("Fuck", 1);
                }
            }
        }

        if (clicked)
        {
            StartCoroutine(CheckNote());
        }
        if (Songdata.barNumber > currentNoteToCheck && nextNoteToCheck != -1)
        {
            currentIndex++;
            currentNoteToCheck = nextNoteToCheck;
            healthBar.SubtractHp();
            if (currentIndex < notesToCheck.Count - 1)
            {
                nextNoteToCheck = notesToCheck[currentIndex + 1];
            }
            else
            {
                nextNoteToCheck = -1;
            }
        }

    }

    IEnumerator SetClicked()
    {
        clicked = true;

        yield return new WaitForSeconds(0.1f);

        clicked = false;
    }

    IEnumerator CheckNote()
    {
        if (Songdata.barNumber == currentNoteToCheck)
        {
            colliding = true;
            arrowAnimator.SetFloat("Fuck", 2);
            Destroy(notes[currentIndex]);
            bfAnimator.SetBool("Singing", true);
            bfAnimator.SetFloat("Blend", bfSingValue);
            healthBar.AddHp();
            currentIndex++;
            currentNoteToCheck = nextNoteToCheck;
            if (currentIndex < notesToCheck.Count - 1)
            {
                nextNoteToCheck = notesToCheck[currentIndex + 1];
            }
            else
            {
                nextNoteToCheck = -1;
            }

            yield return new WaitForSeconds(0.1f);

            colliding = false;
        }
    }
    
    /*IEnumerator DestroyNote(GameObject noteObject)
    {
        Destroy(noteObject);
        colliding = true;
        arrowAnimator.SetFloat("Fuck", 2);
        bfAnimator.SetBool("Singing", true);
        bfAnimator.SetFloat("Blend", bfSingValue);
        healthBar.AddHp();

        yield return new WaitForSeconds(0.1f);
        
        colliding = false;

    }*/
}
