using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleHoldNote : MonoBehaviour
{
    public int HoldNoteTime = 0;
    [HideInInspector]public Toggle toggle;
    [HideInInspector]public HoldNoteMenu noteMenu;
    public GameObject holdNoteThing;

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
        AddUiElement();
    }

    public void SubtractFromCounter()
    {
        if (HoldNoteTime > 0)
        {
            HoldNoteTime--;
            AddUiElement();
        }
    }

    public void AddUiElement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("HoldNote"))
            {
                Destroy(GetComponent<Transform>().GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < HoldNoteTime; i++)
        {
            GameObject swagName;
            swagName = Instantiate(holdNoteThing, this.transform);
        }
    }
}
