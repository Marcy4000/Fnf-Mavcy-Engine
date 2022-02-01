using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoldNoteMenu : MonoBehaviour
{
    public GameObject lastPlacedNote = null;
    public TMP_Text counter;

    // Update is called once per frame
    void Update()
    {
        if (lastPlacedNote != null)
        {
            counter.text = "" + lastPlacedNote.GetComponent<ToggleHoldNote>().HoldNoteTime;
        }
    }

    public void AddToCounter()
    {
        if (lastPlacedNote == null)
            return;
        lastPlacedNote.GetComponent<ToggleHoldNote>().AddToCounter();
    }

    public void SubtToCounter()
    {
        if (lastPlacedNote == null)
            return;
        lastPlacedNote.GetComponent<ToggleHoldNote>().SubtractFromCounter();
    }

    public void SetCurrentNote(GameObject note)
    {
        lastPlacedNote = note;
    }
}
