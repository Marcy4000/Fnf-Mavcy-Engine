using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHoldNote : MonoBehaviour
{
    public int HoldNoteTime = 0;
    
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
