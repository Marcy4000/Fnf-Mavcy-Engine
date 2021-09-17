using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public bool isHoldNote;
    public int holdTime;
    public GameObject holdNoteThing;
    public List<HoldArrow> holdArrows;

    private void OnEnable()
    {
        if (isHoldNote)
        {
            StartCoroutine(CreateNoteThing());
        }
        else
        {
            return;
        }
    }

    IEnumerator CreateNoteThing()
    {
        VerticalLayoutGroup layoutGroup;
        layoutGroup = this.GetComponent<VerticalLayoutGroup>();
        yield return new WaitForSecondsRealtime(0.35f);
        
        for (int i = 0; i < holdTime; i++)
        {
            GameObject swagName;
            swagName = Instantiate(holdNoteThing, this.transform);
            holdArrows.Add(swagName.GetComponent<HoldArrow>());
        }
        yield return new WaitForSecondsRealtime(0.1f);
        
        for (int i = 0; i < holdArrows.Count; i++)
        {
            GoUp script;
            script = holdArrows[i].gameObject.GetComponent<GoUp>();
            holdArrows[i].DeChildObject();
            script.enabled = true;
        }
        layoutGroup.enabled = false;
        
    }
}
