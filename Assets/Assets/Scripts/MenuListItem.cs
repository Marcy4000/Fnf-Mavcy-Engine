using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListItem : MonoBehaviour
{
    public int ID;

    public void SetValue(bool value)
    {
        GlobalDataSfutt.characterActive[ID] = value;
    }
}
