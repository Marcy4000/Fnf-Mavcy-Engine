using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleHoldNote : MonoBehaviour
{
    public int HoldNoteTime = 0;
    public Toggle toggle;
    public HoldNoteMenu noteMenu;

    private void Start()
    {
        toggle = this.GetComponent<Toggle>();
        noteMenu = GameObject.Find("NoteMenu").GetComponent<HoldNoteMenu>();
    }

    private void Update()
    {
        if (toggle.isOn == false && HoldNoteTime != 0)
        {
            HoldNoteTime = 0;
        }

        if (EventSystem.current.currentSelectedGameObject == toggle.gameObject)
        {
            noteMenu.SetCurrentNote(this.gameObject);
        }
    }

    public void AddToCounter()
    {
        HoldNoteTime++;
    }

    public void SubtractFromCounter()
    {
        if (HoldNoteTime > 0)
        {
            HoldNoteTime--;
        }
    }
}
