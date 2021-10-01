using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SectionMenager : MonoBehaviour
{
    public TMP_Text counter;
    public TMP_InputField bpm;

    public int sectionId;
    public int highestSectionIdSelected;

    public ExportSong exportSong;

    public List<GameObject> sectionView;
    public GameObject sectionViewPrefab;
    public Transform parent;

    private void Start()
    {
        GameObject stuff;
        stuff = GameObject.Find("Debug menu");
        exportSong = stuff.GetComponent<ExportSong>();
        
        for (int i = 0; i < 99; i++)
        {
            //Creating 99 sections and adding them to an array
            sectionView.Add(Instantiate(sectionViewPrefab, parent));
            SectionId section;
            section = sectionView[i].GetComponent<SectionId>();
            section.currentSectionId = i;
            exportSong.leftSection = GameObject.FindGameObjectsWithTag("LeftSection");
            exportSong.downSection = GameObject.FindGameObjectsWithTag("DownSection");
            exportSong.upSection = GameObject.FindGameObjectsWithTag("UpSection");
            exportSong.rightSection = GameObject.FindGameObjectsWithTag("RightSection");
        }
        for (int i = 0; i < 99; i++)
        {
            //then deactivate every section that isn't selected
            if (i != sectionId)
            {
                sectionView[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //failcheck so we don't end up with negative sections, because that would be bad
        if (sectionId < 0)
        {
            sectionId = 0;
        }
        if (sectionId > highestSectionIdSelected)
        {
            highestSectionIdSelected = sectionId;
        }
        counter.text = "" + sectionId;
        
    }

    public void CallFuncion()
    {
        SectionId section;
        section = sectionView[sectionId].GetComponent<SectionId>();
        section.Clear();
    }

    public void AddSectionId()
    {
        if (sectionId < 99)
        {
            sectionId++;
        }
        for (int i = 0; i < 99; i++)
        {
            if (i != sectionId)
            {
                sectionView[i].SetActive(false);
            }
            else
            {
                sectionView[i].SetActive(true);
            }
        }
    }

    public void SubtractSectionId()
    {
        if (sectionId > 0)
        {
            sectionId--;
        }
        for (int i = 0; i < 99; i++)
        {
            if (i != sectionId)
            {
                sectionView[i].SetActive(false);
            }
            else
            {
                sectionView[i].SetActive(true);
            }
        }
    }
}
