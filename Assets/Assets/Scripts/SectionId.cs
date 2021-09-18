using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionId : MonoBehaviour
{
    public int currentSectionId;

    public CreateSectionLeft sectionLeft;
    public CreateSectionDown sectionDown;
    public CreateSectionUp sectionUp;
    public CreateSectionRight sectionRight;
    
    public void Clear()
    {
        sectionLeft.ClearSection();
        sectionDown.ClearSection();
        sectionUp.ClearSection();
        sectionRight.ClearSection();
    }
}
