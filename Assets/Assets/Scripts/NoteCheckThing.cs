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

    public HealthBar healthBar;

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject note;
        note = collision.gameObject;

        if (clicked)
        {
            StartCoroutine(DestroyNote(note));
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToCheck))
        {
            StartCoroutine(SetClicked());
        }
        if (Input.GetKeyUp(keyToCheck))
        {
            arrowAnimator.SetFloat("Fuck", 0);
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
    }

    IEnumerator SetClicked()
    {
        clicked = true;

        yield return new WaitForSeconds(0.1f);

        clicked = false;
    }

    IEnumerator DestroyNote(GameObject noteObject)
    {
        Destroy(noteObject);
        colliding = true;
        arrowAnimator.SetFloat("Fuck", 2);
        bfAnimator.SetBool("Singing", true);
        bfAnimator.SetFloat("Blend", bfSingValue);
        healthBar.AddHp();

        yield return new WaitForSeconds(0.1f);
        
        colliding = false;
        bfAnimator.SetBool("Singing", false);

    }
}
